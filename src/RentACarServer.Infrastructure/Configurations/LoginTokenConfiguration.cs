using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACarServer.Domain.LoginTokens;

namespace RentACarServer.Infrastructure.Configurations;

internal sealed class LoginTokenConfiguration : IEntityTypeConfiguration<LoginToken>
{
    public void Configure(EntityTypeBuilder<LoginToken> builder)
    {
        builder.HasKey(i => i.Id);
        builder.OwnsOne(i => i.IsActive);
        builder.OwnsOne(i => i.Token);
        builder.OwnsOne(i => i.UserId);
        builder.OwnsOne(i => i.ExpiresDate);

    }
}
