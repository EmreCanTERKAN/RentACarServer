using Microsoft.EntityFrameworkCore;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Role;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Roles;

[Permission("role:view")]
public sealed record RoleGetQuery(
    Guid Id) : IRequest<Result<RoleDto>>;

internal sealed class RoleGetQueryHandler(
    IRoleRepository roleRepository) : IRequestHandler<RoleGetQuery, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(RoleGetQuery request, CancellationToken cancellationToken)
    {
        var role = await roleRepository
            .GetAllWithAudit()
            .MapTo()
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (role is null)
        {
            return Result<RoleDto>.Failure("Rol bulunamadı");
        }

        return role;
    }
}
