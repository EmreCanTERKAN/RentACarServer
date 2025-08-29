using RentACarServer.Application.Behaviors;
using System.Reflection;

namespace RentACarServer.Application.Service;
public sealed class PermissionService
{
    // İzinleri sadece bir kez okunacak ve burada saklanacak.
    private readonly IReadOnlyList<string> _permissions;

    public PermissionService()
    {
        // Bu kod uygulama başlarken SADECE BİR KEZ çalışacak.
        var permissions = new HashSet<string> { "dashboard:view" };
        var assembly = Assembly.GetExecutingAssembly();

        var permissionsFromAttributes = assembly.GetTypes()
            .Select(t => t.GetCustomAttribute<PermissionAttribute>())
            .Where(attr => attr is not null && !string.IsNullOrEmpty(attr.Permission))
            .Select(attr => attr!.Permission);

        foreach (var permission in permissionsFromAttributes)
        {
            permissions.Add(permission!);
        }

        _permissions = permissions.ToList().AsReadOnly();
    }

    // Bu metot artık anında cevap döner, çünkü hesaplama çoktan yapıldı.
    public IReadOnlyList<string> GetAll()
    {
        return _permissions;
    }
}
