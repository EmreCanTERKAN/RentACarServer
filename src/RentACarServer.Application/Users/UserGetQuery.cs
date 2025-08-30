using Microsoft.EntityFrameworkCore;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Branches;
using RentACarServer.Domain.Roles;
using RentACarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Users;
[Permission("user:view")]
public sealed record UserGetQuery(Guid Id) : IRequest<Result<UserDto>>;

internal sealed class UserGetQueryHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IBranchRepository branchRepository) : IRequestHandler<UserGetQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(UserGetQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository
            .GetAllWithAudit()
            .MapTo(roleRepository.GetAll(),branchRepository.GetAll())
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result<UserDto>.Failure("Kullanıcı bulunamadı");
        }

        return user;
    }
}
