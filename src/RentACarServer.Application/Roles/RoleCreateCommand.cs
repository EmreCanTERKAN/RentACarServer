using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Roles;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Roles;

[Permission("role:create")]
public sealed record RoleCreateCommand(
    string Name,
    bool IsActive) : IRequest<Result<string>>;


public sealed class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
{
    public RoleCreateCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Role ismi boş olamaz");
    }
}


internal sealed class RoleCreateCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RoleCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        var isNameExist = await roleRepository.AnyAsync(x => x.Name.Value == request.Name, cancellationToken);

        if (isNameExist)
        {
            return Result<string>.Failure("Bu isme ait başka bir kayıt mevcuttur");
        }

        Name name = new(request.Name);
        Role role = new(name, request.IsActive);

        roleRepository.Add(role);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rol başarıyla oluşturuldu";
    }
}
