using RentACarServer.Application.Users;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.WebApi.Modules;

public static class UserModule
{
    public static void MapUser(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/users")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("Users");

        app.MapPost(string.Empty,
            async (UserCreateCommand request, ISender sender, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();

        app.MapPut(string.Empty,
            async (UserUpdateCommand request, ISender sender, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();

        app.MapDelete("{id}",
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new UserDeleteCommand(id), cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();

        app.MapGet("{id}",
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new UserGetQuery(id), cancellationToken);
            return response.IsSuccessful ? Results.Ok() : Results.InternalServerError(response);
        }).Produces<Result<UserDto>>();


    }
}
