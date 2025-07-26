﻿using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Options;
using Scrutor;

namespace RentACarServer.Infrastructure;
public static class ServiceRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailSettingOptions>(configuration.GetSection("MailSettings"));
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtSetupOptions>();
        services.AddAuthentication().AddJwtBearer();
        services.AddAuthorization();

        using var scoped = services.BuildServiceProvider().CreateScope();
        var mailSettings = scoped.ServiceProvider.GetRequiredService<IOptions<MailSettingOptions>>();
        if (string.IsNullOrEmpty(mailSettings.Value.UserId))
        {
            services
                .AddFluentEmail(mailSettings.Value.Email)
                .AddSmtpSender(
                        mailSettings.Value.Smtp,
                        mailSettings.Value.Port);
        }
        else
        {
            services
                .AddFluentEmail(mailSettings.Value.Email)
                .AddSmtpSender(
                        mailSettings.Value.Smtp,
                        mailSettings.Value.Port,
                        mailSettings.Value.UserId,
                        mailSettings.Value.Password);
        }


        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            string con = configuration.GetConnectionString("SqlServer")!;
            opt.UseSqlServer(con);
        });

        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

        services.Scan(action => action
        .FromAssemblies(typeof(ServiceRegistrar).Assembly)
        .AddClasses(publicOnly: false)
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        .AsImplementedInterfaces()
        .WithScopedLifetime()
        );


        return services;
    }
}
