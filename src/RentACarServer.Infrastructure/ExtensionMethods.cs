using Microsoft.EntityFrameworkCore;
using RentACarServer.Domain.Abstractions;
using System.Linq.Expressions;

namespace RentACarServer.Infrastructure;
public static class ExtensionMethods
{
    public static void ApplyGlobalFilters(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (typeof(Entity).IsAssignableFrom(clrType))
            {
                var parameter = Expression.Parameter(clrType, "e");
                var property = Expression.Property(parameter, nameof(Entity.IsDeleted));
                var condion = Expression.Equal(property, Expression.Constant(false));
                var lamda = Expression.Lambda(condion, parameter);

                entityType.SetQueryFilter(lamda);
            }
        }
    }
}
