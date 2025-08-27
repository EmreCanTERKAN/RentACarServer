using RentACarServer.Domain.Branches;
using RentACarServer.Domain.Role;
using RentACarServer.Domain.Users;
using TS.MediatR;

namespace RentACarServer.Application.Users;
public sealed record UserGetAllQuery : IRequest<IQueryable<UserDto>>;


internal sealed class UserGetAllQueryHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IBranchRepository branchRepository) : IRequestHandler<UserGetAllQuery, IQueryable<UserDto>>
{
    public Task<IQueryable<UserDto>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
    {
        var res = userRepository.GetAllWithAudit().MapTo(roleRepository.GetAll(), branchRepository.GetAll());

        return Task.FromResult(res);
    }
}
