using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.ProtectionPackages;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.ProtectionPackages;

[Permission("protection_package:delete")]
public sealed record ProtectionPackageDeleteCommand(
    Guid Id) : IRequest<Result<string>>;


internal sealed class ProtectionPackageDeleteCommandHandler(
    IProtectionPackageRepository protectionPackageRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ProtectionPackageDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ProtectionPackageDeleteCommand request, CancellationToken cancellationToken)
    {
        var package = await protectionPackageRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (package is null)
        {
            return Result<string>.Failure("Koruma paketi bulunamadı");
        }

        package.Delete();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Koruma paketi başarıyla silindi";
    }
}