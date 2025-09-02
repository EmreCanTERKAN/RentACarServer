using RentACarServer.Domain.ProtectionPackages;
using RentACarServer.Infrastructure.Abstractions;
using RentACarServer.Infrastructure.Context;

namespace RentACarServer.Infrastructure.Repositories;
internal sealed class ProtectionPackageRepository : AuditableRepository<ProtectionPackage, ApplicationDbContext>, IProtectionPackageRepository
{
    public ProtectionPackageRepository(ApplicationDbContext context) : base(context)
    {
    }
}
