using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Categories;
using TS.MediatR;

namespace RentACarServer.Application.Categories;
[Permission("category:view")]
public sealed record CategoryGetAllQuery : IRequest<IQueryable<CategoryDto>>;

internal sealed class CategoryGetAllQueryHandler(
    ICategoryRepository categoryRepository) : IRequestHandler<CategoryGetAllQuery, IQueryable<CategoryDto>>
{
    public Task<IQueryable<CategoryDto>> Handle(CategoryGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = categoryRepository.GetAllWithAudit().MapTo();

        return Task.FromResult(response);
    }
}
