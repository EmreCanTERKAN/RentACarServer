using RentACarServer.Application.Behaviors;
using System.Reflection;

namespace RentACarServer.Application.Service;
public sealed class PermissionService
{
    public List<string> GetAll()
    {
        var permission = new HashSet<string>();

        var assembly = Assembly.GetExecutingAssembly();

        var types = assembly.GetTypes();

        foreach (var type in types)
        {
            var permissionAttr = type.GetCustomAttribute<PermissionAttribute>();

            if (permissionAttr is not null && !string.IsNullOrEmpty(permissionAttr.Permission))
            {
                permission.Add(permissionAttr.Permission);
            }
        }

        return permission.ToList();
    }
}
