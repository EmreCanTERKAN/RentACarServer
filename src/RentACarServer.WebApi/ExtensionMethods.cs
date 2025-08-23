using GenericRepository;
using RentACarServer.Application.Service;
using RentACarServer.Domain.Abstractions;
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

        Branch? branch = await branchRepository.FirstOrDefaultAsync(i => i.Name.Value == "Merkez Şube");
        Role? role = await roleRepository.FirstOrDefaultAsync(i => i.Name.Value == "sys_admin");

        if (branch is null)
        {
            Name name = new("Merkez Şube");
            Address address = new(
                "Ankara",
                "Altındağ",
                "Cebeci 4yol");
            Contact contact = new(
                "3124368263",
                "3124368264",
                "info@rentcar.com");
            branch = new(name, address, contact, true);
            branchRepository.Add(branch);
        }

        if (role is null)
        {
            Name name = new("sys_admin");
            role = new(name, true);
            roleRepository.Add(role);
        }

        if (!(await userRepository.AnyAsync(p => p.UserName.Value == "admin")))
        {
            FirstName firstName = new("Emre");
            LastName lastName = new("TERKAN");
            Email email = new("emrecanterkan@gmail.com");
            UserName userName = new("admin");
            Password password = new("1");
            IdentityId branchId = branch.Id;
            IdentityId roleId = role.Id;

            var user = new User(
                firstName,
                lastName,
                email,
                userName,
                password,
                branchId,
                roleId);

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