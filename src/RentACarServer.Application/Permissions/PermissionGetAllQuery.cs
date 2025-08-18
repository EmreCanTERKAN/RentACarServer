using RentACarServer.Application.Behaviors;
using RentACarServer.Application.Service;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Permissions;

[Permission("permission:view")]
public sealed record PermissionGetAllQuery : IRequest<Result<List<string>>>;

internal sealed class PermissionGetAllQueryHandler(
    PermissionService permissionService) : IRequestHandler<PermissionGetAllQuery, Result<List<string>>>
{
    public Task<Result<List<string>>> Handle(PermissionGetAllQuery request, CancellationToken cancellationToken)
    {
        var list = permissionService.GetAll();

        return Task.FromResult(Result<List<string>>.Succeed(list));
    }
}
