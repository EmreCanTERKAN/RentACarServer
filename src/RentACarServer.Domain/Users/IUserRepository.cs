using RentACarServer.Domain.Abstractions;

namespace RentACarServer.Domain.Users;
public interface IUserRepository : IAuditableRepository<User>
{
}
