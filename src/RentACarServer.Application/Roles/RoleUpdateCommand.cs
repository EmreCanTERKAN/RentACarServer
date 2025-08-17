using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Role;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Roles;

[Permission("role:edit")]
public sealed record RoleUpdateCommand(
    Guid Id,
    string Name,
    bool IsActive) : IRequest<Result<string>>;

public sealed class RoleUpdateCommandValidator : AbstractValidator<RoleUpdateCommand>
{
    public RoleUpdateCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Rol ismi boş olamaz");
    }
}

internal sealed class RoleUpdateCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RoleUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (role is null)
        {
            return Result<string>.Failure("Bu id'ye ait rol bulunamadı");
        }

        var isNameExist = await roleRepository.AnyAsync(x => x.Name.Value == request.Name, cancellationToken);

        if (isNameExist)
        {
            return Result<string>.Failure("Bu rol daha önce kayıt edilmiştir");
        }

        Name name = new(request.Name);
        role.SetName(name);
        role.SetStatus(request.IsActive);

        roleRepository.Update(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rol başarıyla güncellendi";
    }
}
