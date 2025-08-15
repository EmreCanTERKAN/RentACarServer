using RentACarServer.Domain.Role;
using RentACarServer.Infrastructure.Abstractions;
using RentACarServer.Infrastructure.Context;

namespace RentACarServer.Infrastructure.Repositories;

internal sealed class RoleRepository : AuditableRepository<Role, ApplicationDbContext>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}