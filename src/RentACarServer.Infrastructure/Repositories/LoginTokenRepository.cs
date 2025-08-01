using GenericRepository;
using RentACarServer.Domain.LoginTokens;
using RentACarServer.Infrastructure.Context;

namespace RentACarServer.Infrastructure.Repositories;
internal sealed class LoginTokenRepository : Repository<LoginToken, ApplicationDbContext>, ILoginTokenRepository
{
    public LoginTokenRepository(ApplicationDbContext context) : base(context)
    {
    }
}
