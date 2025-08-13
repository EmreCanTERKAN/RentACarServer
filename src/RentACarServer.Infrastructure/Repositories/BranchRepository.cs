using RentACarServer.Domain.Branches;
using RentACarServer.Infrastructure.Abstractions;
using RentACarServer.Infrastructure.Context;

namespace RentACarServer.Infrastructure.Repositories;
internal sealed class BranchRepository : AuditableRepository<Branch, ApplicationDbContext>, IBranchRepository
{
    public BranchRepository(ApplicationDbContext context) : base(context)
    {
    }
}
