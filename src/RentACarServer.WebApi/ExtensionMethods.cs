using GenericRepository;
using RentACarServer.Application.Service;
using RentACarServer.Domain.Users;
using RentACarServer.Domain.Users.ValueObjects;

namespace RentACarServer.WebApi;

public static class ExtensionMethods
{
    public static async Task CreateFirstUser(this WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var userRepository = scoped.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();

        if (!(await userRepository.AnyAsync(x => x.UserName.Value == "admin")))
        {
            FirstName firstName = new("Emre");
            LastName lastName = new("TERKAN");
            Email email = new("emrecan@hotmail.comm");
            UserName userName = new("admin");
            Password password = new("1");

            var user = new User(firstName, lastName, email, userName, password);

            userRepository.Add(user);
            await unitOfWork.SaveChangesAsync();
        }
    }

    public static async Task CleanRemovedPermissionsFromRoleAsync (this WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var srv = scoped.ServiceProvider;
        var service = srv.GetRequiredService<PermissionCleanerService>();
        await service.CleanRemovedPermissionsFromRolesAsync();
    }
}
