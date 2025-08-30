using FluentValidation;
using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Categories;
using RentACarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Categories;

[Permission("category:create")]
public sealed record CategoryCreateCommand(
    string Name,
    bool IsActive) : IRequest<Result<string>>;

public sealed class CategoryCreateCommandValidator : AbstractValidator<CategoryCreateCommand>
{
    public CategoryCreateCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori ismi boş olamaz");
    }
}


internal sealed class CategoryCreateCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CategoryCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        var isNameExist = await categoryRepository.AnyAsync(p => p.Name.Value == request.Name, cancellationToken);

        if (isNameExist)
        {
            return Result<string>.Failure("Bu isme ait başka bir kayıt mevcuttur");
        }

        Name name = new(request.Name);

        Category category = new(name, request.IsActive);

        await categoryRepository.AddAsync(category,cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kategori başarıyla eklendi";
    }
}
