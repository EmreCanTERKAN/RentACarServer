using GenericRepository;
using RentACarServer.Application.Service;
using RentACarServer.Domain.Branches;
using RentACarServer.Domain.Role;
using RentACarServer.Domain.Shared;
using RentACarServer.Domain.Users;
using RentACarServer.Domain.Users.ValueObjects;

namespace RentACarServer.WebApi;

public static class ExtensionMethods
{
    public static async Task CreateFirstUser(this WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var srv = scoped.ServiceProvider;

        var userRepository = srv.GetRequiredService<IUserRepository>();
        var roleRepository = srv.GetRequiredService<IRoleRepository>();
        var branchRepository = srv.GetRequiredService<IBranchRepository>();

        var unitOfWork = srv.GetRequiredService<IUnitOfWork>();

        Branch? branch = await branchRepository.FirstOrDefaultAsync(x => x.Name.Value == "Merkez Şube");

        Role? role = await roleRepository.FirstOrDefaultAsync(x => x.Name.Value == "system_admin");

        if(branch is null)
        {
            Name name = new("Merkez Şube");
            Address address = new("Ankara", "ALTINDAĞ", "Talatpaşa bulvarı 151/6");
            Contact contact = new("03124468263", "03124468264", "info@rentAcar");
            branch = new Branch(name, address, contact,true);

            branchRepository.Add(branch);
        }

        if(role is null)
        {
            Name roleName = new("system_admin");
            role = new Role(roleName, true);

            roleRepository.Add(role);
        }
        
        if (!(await userRepository.AnyAsync(x => x.UserName.Value == "admin")))
        {
            FirstName firstName = new("Emre");
            LastName lastName = new("TERKAN");
            Email email = new("emrecan@hotmail.comm");
            UserName userName = new("admin");
            Password password = new("1");

            var user = new User(firstName, lastName, email, userName, password,role.Id,branch.Id);

            userRepository.Add(user);
            await unitOfWork.SaveChangesAsync();
        }
    }

    public static async Task CleanRemovedPermissionsFromRoleAsync(this WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var srv = scoped.ServiceProvider;
        var service = srv.GetRequiredService<PermissionCleanerService>();
        await service.CleanRemovedPermissionsFromRolesAsync();
    }
}
