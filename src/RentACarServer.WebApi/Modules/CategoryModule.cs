using RentACarServer.Application.Categories;
using RentACarServer.Domain.Categories;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.WebApi.Modules;

public static class CategoryModule
{
    public static void MapCategory(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/categories")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("Categories");

        app.MapPost(string.Empty,
            async (CategoryCreateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(request, cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapPut(string.Empty,
            async (CategoryUpdateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(request, cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapDelete("{id}",
            async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(new CategoryDeleteCommand(Id), cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapGet("{id}",
            async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(new CategoryGetQuery(Id), cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<Category>>();
    }
}
