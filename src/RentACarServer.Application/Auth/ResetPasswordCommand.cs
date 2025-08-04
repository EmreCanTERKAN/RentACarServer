using FluentValidation;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentACarServer.Domain.LoginTokens;
using RentACarServer.Domain.Users;
using RentACarServer.Domain.Users.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Auth;
public sealed record ResetPasswordCommand(
    Guid ForgotPasswordCode,
    string NewPassword,
    bool LogoutAllDevice) : IRequest<Result<string>>;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(p => p.NewPassword).NotEmpty().WithMessage("Geçerli bir yeni şifre giriniz");
    }
}

internal sealed class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    ILoginTokenRepository loginTokenRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p =>
        p.ForgotPasswordCode != null &&
        p.ForgotPasswordCode.Value == request.ForgotPasswordCode &&
        p.IsForgotPasswordCompleted.Value == false, cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure("Şifre sıfırlama değeriniz geçersiz");
        }

        var fpDate = user.ForgotPasswordCreatedAt!.Value.AddDays(1);
        var now = DateTimeOffset.Now;

        if (fpDate < now)
        {
            return Result<string>.Failure("Şifre sıfırlama değeriniz geçersiz");
        }

        Password password = new(request.NewPassword);
        user.SetPassword(password);
        userRepository.Update(user);

        if (request.LogoutAllDevice)
        {
        var loginTokens = await loginTokenRepository
            .Where(p => p.UserId == user.Id && p.IsActive.Value == true)
            .ToListAsync(cancellationToken);

        foreach (var token in loginTokens)
        {
            token.SetIsActive(new(false));
        }
        loginTokenRepository.UpdateRange(loginTokens);
        }


        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Şifre başarıyla sıfırlandı.";
    }
}
