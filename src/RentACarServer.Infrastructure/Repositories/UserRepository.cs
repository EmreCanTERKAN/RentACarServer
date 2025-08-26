using GenericRepository;
using RentACarServer.Domain.Users;
using RentACarServer.Infrastructure.Abstractions;
using RentACarServer.Infrastructure.Context;

namespace RentACarServer.Infrastructure.Repositories;
internal sealed class UserRepository : AuditableRepository<User, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
