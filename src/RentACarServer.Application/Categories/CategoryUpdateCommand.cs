using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Categories;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Categories;
[Permission("category:edit")]
public sealed record CategoryUpdateCommand(
    Guid Id,
    string Name,
    bool IsActive) : IRequest<Result<string>>;

public sealed class CategoryUpdateCommandValidator : AbstractValidator<CategoryUpdateCommand>
{
    public CategoryUpdateCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Kategori ismi boş olamaz");
    }
}

internal sealed class CategoryUpdateCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CategoryUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (category is null)
        {
            return Result<string>.Failure("Bu id'ye ait kayıt bulunamadı");
        }
        var isNameExist = await categoryRepository.AnyAsync(p => p.Name.Value == request.Name, cancellationToken);

        if (isNameExist)
        {
            return Result<string>.Failure("Bu isme ait başka bir kayıt mevcuttur");
        }

        category.SetName(new Name(request.Name));
        category.SetStatus(request.IsActive);

        categoryRepository.Update(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Category başarıyla güncellendi";
    }
}

