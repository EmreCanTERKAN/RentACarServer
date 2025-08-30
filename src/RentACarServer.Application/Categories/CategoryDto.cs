using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Categories;

namespace RentACarServer.Application.Categories;
public sealed class CategoryDto : EntityDto
{
    public string Name { get; set; } = default!;
}

public static class CategoryExtensions
{
    public static IQueryable<CategoryDto> MapTo(this IQueryable<EntityWithAuditDto<Category>> entity)
    {
        var response = entity.Select(s => new CategoryDto
        {
            Id = s.Entity.Id,
            Name = s.Entity.Name.Value,
            CreatedAt = s.Entity.CreatedAt,
            CreatedBy = s.Entity.CreatedBy,
            IsActive = s.Entity.IsActive,
            UpdatedAt = s.Entity.UpdatedAt,
            UpdatedBy = s.Entity.UpdatedBy == null ? null : s.Entity.UpdatedBy.Value,
            CreatedFullName = s.CreatedUser.FullName.Value,
            UpdatedFullName = s.UpdatedUser == null ? null : s.UpdatedUser.FullName.Value
        });

        return response;
    }
}

