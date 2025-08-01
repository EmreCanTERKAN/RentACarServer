using RentACarServer.Domain.Users;

namespace RentACarServer.Application.Service;
public interface IJwtProvider
{
    Task<string> CreateTokenAsync(User user,CancellationToken cancellationToken = default);
}
