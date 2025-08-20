using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACarServer.Domain.Branches;

namespace RentACarServer.Infrastructure.Configurations;

internal sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");
        builder.HasKey(i => i.Id);
        builder.OwnsOne(i => i.Name);
        builder.OwnsOne(i => i.Address);
        builder.OwnsOne(i => i.Contact);
    }
}
