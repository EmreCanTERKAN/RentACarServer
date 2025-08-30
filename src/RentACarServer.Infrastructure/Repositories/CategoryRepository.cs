using RentACarServer.Domain.Categories;
using RentACarServer.Infrastructure.Abstractions;
using RentACarServer.Infrastructure.Context;

namespace RentACarServer.Infrastructure.Repositories;
internal sealed class CategoryRepository : AuditableRepository<Category, ApplicationDbContext>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}
