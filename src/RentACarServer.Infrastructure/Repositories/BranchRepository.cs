using GenericRepository;
using RentACarServer.Domain.Branches;
using RentACarServer.Infrastructure.Context;

namespace RentACarServer.Infrastructure.Repositories;
internal sealed class BranchRepository : Repository<Branch, ApplicationDbContext>, IBranchRepository
{
    public BranchRepository(ApplicationDbContext context) : base(context)
    {
    }
}
