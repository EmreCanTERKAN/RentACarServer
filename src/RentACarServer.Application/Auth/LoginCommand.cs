using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Service;
using RentACarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Auth;
public sealed record LoginCommand(
    string EmailOrUserName,
    string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse
{
    public string? Token { get; set; }
    public string? TFACode { get; set; }
}

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(p => p.EmailOrUserName).NotEmpty().WithMessage("Geçerli bir mail ya da kullanıcı adı girin");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Geçerli bir şifre girin");
    }
}

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IMailService mailService,
    IJwtProvider jwtProvider,
    IUnitOfWork unitOfWork) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(x => x.UserName.Value == request.EmailOrUserName || x.Email.Value == request.EmailOrUserName, cancellationToken);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı adı ya da şifre yanlış");
        }

        var checkPassword = user.VerifyPasswordHash(request.Password);

        if (!checkPassword)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı adı ya da şifre yanlış");
        }

        if (!user.TFAStatus.Value)
        {
            var token = await jwtProvider.CreateTokenAsync(user, cancellationToken);
            var res = new LoginCommandResponse { Token = token };
            return res;
        }
        else
        {
            user.CreateTFACode();

            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            string to = user.Email.Value;
            string subject = "Giriş Onay";
            string body = $@"Uygulamaya girmek için aşağıdaki kodu kullanabilirsiniz. Bu kod sadece 5 dk geçerlidir. <p><h4>{user.TFAConfirmCode!.Value}</h4></p>";
            await mailService.SendAsync(to, subject, body, cancellationToken);

            var res = new LoginCommandResponse { TFACode = user.TFACode!.Value };
            return res;
        }


    }
}
