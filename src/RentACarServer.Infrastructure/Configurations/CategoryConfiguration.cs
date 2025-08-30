using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACarServer.Domain.Categories;

namespace RentACarServer.Infrastructure.Configurations;
internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(i => i.Id);
        builder.OwnsOne(i => i.Name);
    }
}
