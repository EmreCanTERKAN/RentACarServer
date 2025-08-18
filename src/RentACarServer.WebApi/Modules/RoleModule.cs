using RentACarServer.Application.Roles;
using RentACarServer.Domain.Role;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.WebApi.Modules;

public static class RoleModule
{
    public static void MapRole(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/roles")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("Roles");

        app.MapPost(string.Empty,
            async (RoleCreateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }).Produces<Result<string>>();

        app.MapPut(string.Empty,
            async (RoleUpdateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }).Produces<Result<string>>();

        app.MapDelete("{id}",
            async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new RoleDeleteCommand(Id), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }).Produces<Result<string>>();

        app.MapGet("{id}",
            async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new RoleGetQuery(Id), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }).Produces<Result<Role>>();

        app.MapPut("update-permissions",
            async (RoleUpdatePermissionsCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }).Produces<Result<string>>();

    }
}
