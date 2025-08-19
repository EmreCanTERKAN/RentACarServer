using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RentACarServer.Application.Behaviors;
using RentACarServer.Application.Service;
using TS.MediatR;

namespace RentACarServer.Application;
public static class ServiceRegistrar
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<PermissionService>();
        services.AddScoped<PermissionCleanerService>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceRegistrar).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(PermissionBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ServiceRegistrar).Assembly);

        return services;
    }
}
