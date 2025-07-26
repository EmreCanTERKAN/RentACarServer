using FluentValidation;
using RentACarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Auth;
public sealed record ForgotPasswordCommand(
    string Email) : IRequest<Result<string>>;

public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email adresi boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir mail adresi girin");
    }
}

internal sealed class ForgotPasswordCommandHandler(
    IUserRepository userRepository) : IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(x => x.Email.Value == request.Email, cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı");
        }

        //şifre sıfırlama maili

        return "Şifre sıfırlama maili gönderildi";
    }
}
