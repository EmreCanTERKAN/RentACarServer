using RentACarServer.Application.Permissions;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.WebApi.Modules;

public static class PermissionModule
{
    public static void MapPermission(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/permissions")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("Permissions");

        app.MapGet(string.Empty,
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new PermissionGetAllQuery(), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }).Produces<Result<List<string>>>();
    }
}
