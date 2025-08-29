using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Application.Service;
using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Users;
using RentACarServer.Domain.Users.ValueObjects;
using TS.MediatR;
using TS.Result;

[Permission("user:edit")]
public sealed record UserUpdateCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    Guid? BranchId,
    Guid RoleId,
    bool IsActive) : IRequest<Result<string>>;

public sealed class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
{
    public UserUpdateCommandValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().WithMessage("Geçerli bir ad girin");
        RuleFor(p => p.LastName).NotEmpty().WithMessage("Geçerli bir soyad girin");
        RuleFor(p => p.UserName).NotEmpty().WithMessage("Geçerli bir kullanıcı adı girin");
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Geçerli bir mail adresi girin")
            .EmailAddress().WithMessage("Geçerli bir mail adresi girin");
    }
}

internal sealed class UserUpdateCommandHandler(
    IUserRepository userRepository,
    IClaimContext claimContext,
    IUnitOfWork unitOfWork) : IRequestHandler<UserUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {

        var user = await userRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı");
        }

        if (request.Email != user.Email.Value)
        {
            var emailExists = await userRepository.AnyAsync(p => p.Email.Value == request.Email, cancellationToken);
            if (emailExists)
            {
                return Result<string>.Failure("Bu mail adresi daha önce kullanılmış");
            }
        }

        if (request.UserName != user.UserName.Value)
        {
            var userNameExists = await userRepository.AnyAsync(p => p.UserName.Value == request.UserName, cancellationToken);
            if (userNameExists)
            {
                return Result<string>.Failure("Bu kullanıcı adı daha önce kullanılmış");
            }
        }


        var branchId = claimContext.GetBranchId();
        if (request.BranchId is not null)
        {
            branchId = request.BranchId.Value;
        }
        user.SetFirstName(new FirstName(request.FirstName));
        user.SetLastName(new LastName(request.LastName));
        user.SetEmail(new Email(request.Email));
        user.SetUserName(new UserName(request.UserName));
        user.SetBranchId(new IdentityId(branchId));
        user.SetRoleId(new IdentityId(request.RoleId));
        user.SetStatus(request.IsActive);
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı başarıyla güncellendi";
    }
}