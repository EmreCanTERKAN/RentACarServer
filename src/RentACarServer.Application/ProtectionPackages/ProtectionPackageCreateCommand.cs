using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.ProtectionPackages;
using RentACarServer.Domain.ProtectionPackages.ValueObjects;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.ProtectionPackages;

[Permission("protection_package:create")]
public sealed record ProtectionPackageCreateCommand(
    string Name,
    decimal Price,
    bool IsRecommended,
    IEnumerable<string> Coverages,
    bool IsActive) : IRequest<Result<string>>;

public sealed class ProtectionPackageCreateCommandValidator : AbstractValidator<ProtectionPackageCreateCommand>
{
    public ProtectionPackageCreateCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Geçerli bir paket adı girin");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Geçerli bir fiyat girin");
    }
}

internal sealed class ProtectionPackageCreateCommandHandler(
    IProtectionPackageRepository protectionPackageRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ProtectionPackageCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ProtectionPackageCreateCommand request, CancellationToken cancellationToken)
    {
        var isNameExist = await protectionPackageRepository.AnyAsync(x => x.Name.Value == request.Name, cancellationToken);

        if (isNameExist)
        {
            return Result<string>.Failure("Bu isme ait başka bir kayıt mevcuttur");
        }

        Name name = new(request.Name);
        Price price = new(request.Price);
        IsRecommended isRecommended = new(request.IsRecommended);
        List<ProtectionCoverage> coverages = request.Coverages.Select(s => new ProtectionCoverage(s)).ToList();

        ProtectionPackage protectionPackage = new(name, price, isRecommended, coverages);

        await protectionPackageRepository.AddAsync(protectionPackage, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Koruma paketi başarıyla oluşturuldu";
    }
}