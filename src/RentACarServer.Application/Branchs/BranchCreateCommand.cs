using FluentValidation;
using GenericRepository;
using RentACarServer.Domain.Branchs;
using RentACarServer.Domain.Branchs.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Branchs;
public sealed record BranchCreateCommand(
    string Name,
    Address Address) : IRequest<Result<string>>;

public sealed class BranchCreateCommandValidatior : AbstractValidator<BranchCreateCommand>
{
    public BranchCreateCommandValidatior()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Geçerli bir şube adı girin");
        RuleFor(x => x.Address.City).NotEmpty().WithMessage("Geçerli bir şehir seçin");
        RuleFor(x => x.Address.District).NotEmpty().WithMessage("Geçerli bir ilçe seçin");
        RuleFor(x => x.Address.FullAddress).NotEmpty().WithMessage("Geçerli bir tam adres girin");
        RuleFor(x => x.Address.PhoneNumber1).NotEmpty().WithMessage("Geçerli bir telefon numarası girin");
    }
}

internal sealed class BranchCreateCommandHandler(
    IBranchRepository branchRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<BranchCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BranchCreateCommand request, CancellationToken cancellationToken)
    {
        var isNameExist = await branchRepository.AnyAsync(x => x.Name.Value == request.Name, cancellationToken);

        if (isNameExist)
        {
            return Result<string>.Failure("Bu isme ait başka bir kayıt mevcuttur");
        }

        Name name = new(request.Name);
        Address address = request.Address;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Şube başarıyla oluşturuldu";
    }
}
