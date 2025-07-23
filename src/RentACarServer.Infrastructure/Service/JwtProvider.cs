using RentACarServer.Application.Service;
using RentACarServer.Domain.Users;

namespace RentACarServer.Infrastructure.Service;
internal sealed class JwtProvider : IJwtProvider
{
    public string CreateToken(User user)
    {
        return "token";
    }
}
