using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using RentACarServer.Application.Branches;
using RentACarServer.Application.Categories;
using RentACarServer.Application.Roles;
using RentACarServer.Application.Users;
using TS.MediatR;

namespace RentACarServer.WebApi.Controllers;
[Route("odata")]
[ApiController]
[EnableQuery]
public class AppODataController : ODataController
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        builder.EntitySet<BranchDto>("branches");
        builder.EntitySet<RoleDto>("roles");
        builder.EntitySet<UserDto>("users");
        builder.EntitySet<CategoryDto>("categories");
        return builder.GetEdmModel();
    }

    [HttpGet("branches")]
    public IQueryable<BranchDto> Branches(ISender sender, CancellationToken cancellationToken) =>
        sender.Send(new BranchGetAllQuery(), cancellationToken).Result;


    [HttpGet("roles")]
    public async Task<IQueryable<RoleDto>> Roles(ISender sender, CancellationToken cancellationToken)
    {
        var response =  await sender.Send(new RoleGetAllQuery(), cancellationToken);
        return response;
    }


    [HttpGet("users")]
    public async Task<IQueryable<UserDto>> Users(ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new UserGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("categories")]
    public async Task<IQueryable<CategoryDto>> Categories(ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new CategoryGetAllQuery(), cancellationToken);
        return response;
    }

}
