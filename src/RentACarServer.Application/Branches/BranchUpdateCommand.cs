using FluentValidation;
using GenericRepository;
using RentACarServer.Domain.Branches;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Branches;
public sealed record BranchUpdateCommand(
    Guid Id,
    string Name,
    Address Address,
    bool IsActive) : IRequest<Result<string>>;

public sealed class BranchUpdateCommandValidatior : AbstractValidator<BranchUpdateCommand>
{
    public BranchUpdateCommandValidatior()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Geçerli bir şube adı girin");
        RuleFor(x => x.Address.City).NotEmpty().WithMessage("Geçerli bir şehir seçin");
        RuleFor(x => x.Address.District).NotEmpty().WithMessage("Geçerli bir ilçe seçin");
        RuleFor(x => x.Address.FullAddress).NotEmpty().WithMessage("Geçerli bir tam adres girin");
        RuleFor(x => x.Address.PhoneNumber1).NotEmpty().WithMessage("Geçerli bir telefon numarası girin");
    }
}

internal sealed class BranchUpdateCommandHandler(
    IBranchRepository branchRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<BranchUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BranchUpdateCommand request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (branch is null)
        {
            return Result<string>.Failure("Şube bulunamadı");
        }

        Name name = new(request.Name);
        Address address = request.Address;

        branch.SetName(name);
        branch.SetAddress(address);
        branch.SetStatus(request.IsActive);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Şube başarıyla güncellendi";

    }
}
