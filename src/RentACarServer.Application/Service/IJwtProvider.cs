using RentACarServer.Domain.Users;

namespace RentACarServer.Application.Service;
public interface IJwtProvider
{
    string CreateToken(User user);
}
