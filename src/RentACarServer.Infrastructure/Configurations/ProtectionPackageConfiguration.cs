using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACarServer.Domain.ProtectionPackages;

namespace RentACarServer.Infrastructure.Configurations;

internal sealed class ProtectionPackageConfiguration : IEntityTypeConfiguration<ProtectionPackage>
{
    public void Configure(EntityTypeBuilder<ProtectionPackage> builder)
    {
        builder.ToTable("ProtectionPackages");
        builder.HasKey(i => i.Id);
        builder.OwnsOne(i => i.Name);
        builder.OwnsOne(i => i.Price);
        builder.OwnsOne(i => i.IsRecommended);
        builder.OwnsMany(i => i.Coverages);
    }
}
