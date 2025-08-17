using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Role;
using TS.MediatR;

namespace RentACarServer.Application.Roles;

[Permission("role:view")]
public sealed record RoleGetAllQuery() : IRequest<IQueryable<RoleDto>>;

internal sealed class RoleGetAllQueryHandler(
    IRoleRepository roleRepository) : IRequestHandler<RoleGetAllQuery, IQueryable<RoleDto>>
{
    public Task<IQueryable<RoleDto>> Handle(RoleGetAllQuery request, CancellationToken cancellationToken)
    {
        var roles = roleRepository.GetAllWithAudit().MapTo();

        return Task.FromResult(roles);
    }
}