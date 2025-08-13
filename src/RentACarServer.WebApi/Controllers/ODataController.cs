using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using RentACarServer.Application.Branches;
using TS.MediatR;

namespace RentACarServer.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableQuery]
public class ODataController : ControllerBase
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        builder.EntitySet<BranchGetAllQueryResponse>("braunches");
        return builder.GetEdmModel();
    }

    public Task<IQueryable<BranchGetAllQueryResponse>> Branches(ISender sender, CancellationToken cancellationToken) =>
        sender.Send(new BranchGetAllQuery(), cancellationToken);

}
