using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Role;

namespace RentACarServer.Application.Roles;
public sealed class RoleDto : EntityDto
{
    public string Name { get; set; } = default!;
}

public static class RoleExtensions
{
    public static IQueryable<RoleDto> MapTo(this IQueryable<EntityWithAuditDto<Role>> entities)
    {
        var res = entities.Select(s => new RoleDto
        {
            Id = s.Entity.Id,
            Name = s.Entity.Name.Value,
            CreatedAt = s.Entity.CreatedAt,
            CreatedBy = s.Entity.CreatedBy,
            CreatedFullName = s.CreatedUser.FullName.Value,
            UpdatedAt = s.Entity.UpdatedAt,
            UpdatedBy = s.Entity.UpdatedBy == null ? null : s.Entity.UpdatedBy.Value,
            UpdatedFullName = s.UpdatedUser == null ? null : s.UpdatedUser.FullName.Value,
            IsActive = s.Entity.IsActive
        });
        return res;
    }
}
