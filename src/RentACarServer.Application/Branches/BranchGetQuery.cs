using Microsoft.EntityFrameworkCore;
using RentACarServer.Domain.Branches;
using RentACarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Branches;
public sealed record BranchGetQuery(
    Guid Id) : IRequest<Result<BranchDto>>;


internal sealed class BranchQueryHandler(
    IBranchRepository branchRepository) : IRequestHandler<BranchGetQuery, Result<BranchDto>>
{
    public async Task<Result<BranchDto>> Handle(BranchGetQuery request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository
            .GetAllWithAudit()
            .MapTo()
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (branch is null)
        {
            return Result<BranchDto>.Failure("Şube bulunamadı");
        }

        return branch;
    }
}