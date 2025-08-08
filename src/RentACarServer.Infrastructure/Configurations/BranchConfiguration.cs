using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACarServer.Domain.Branchs;

namespace RentACarServer.Infrastructure.Configurations;

internal sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(i => i.Id);
        builder.OwnsOne(i => i.Name);
        builder.OwnsOne(i => i.Address);
    }
}
