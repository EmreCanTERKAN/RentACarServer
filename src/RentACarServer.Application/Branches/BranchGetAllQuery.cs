using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Branches;
using TS.MediatR;

namespace RentACarServer.Application.Branches;

[Permission("branch:view")]
public sealed record BranchGetAllQuery : IRequest<IQueryable<BranchDto>>;

internal sealed class BranchGetAllQueryHandler(
    IBranchRepository branchRepository) : IRequestHandler<BranchGetAllQuery, IQueryable<BranchDto>>
{
    public Task<IQueryable<BranchDto>> Handle(BranchGetAllQuery request, CancellationToken cancellationToken)
    {

        var response = branchRepository
            .GetAllWithAudit()
            .MapTo();

        return Task.FromResult(response);
    }
}
