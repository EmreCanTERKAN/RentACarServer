using RentACarServer.Domain.Abstractions;

namespace RentACarServer.Domain.Categories;
public interface ICategoryRepository  : IAuditableRepository<Category>
{
}
