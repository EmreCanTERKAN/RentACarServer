using Microsoft.EntityFrameworkCore;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Categories;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Categories;
[Permission("category:view")]
public sealed record CategoryGetQuery(
    Guid Id) : IRequest<Result<CategoryDto>>;

internal sealed class CategoryGetQueryHandler(
    ICategoryRepository categoryRepository) : IRequestHandler<CategoryGetQuery, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> Handle(CategoryGetQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository
            .GetAllWithAudit()
            .MapTo()
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            return Result<CategoryDto>.Failure("Kategori bulunamadı");
        }

        return category;
    }
}
