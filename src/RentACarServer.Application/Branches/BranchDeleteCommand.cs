using GenericRepository;
using RentACarServer.Domain.Branches;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Branches;
public sealed record BranchDeleteCommand(
    Guid Id) : IRequest<Result<string>>;


internal sealed class BranchDeleteCommandHandler(
    IBranchRepository branchRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<BranchDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BranchDeleteCommand request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (branch is null)
        {
            return Result<string>.Failure("Şube bulunamadı");
        }

        branch.Delete();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Şube başarıyla silindi";
    }
}
