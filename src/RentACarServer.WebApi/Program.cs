using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using RentACarServer.Application;
using RentACarServer.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
#region RateLimit
builder.Services.AddRateLimiter(cfr =>
{
    cfr.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.QueueLimit = 100;
        opt.QueueLimit = 100;
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});
#endregion
#region OData
builder.Services
    .AddControllers()
    .AddOData(opt =>
    opt.Select()
       .Filter()
       .Count()
       .Expand()
       .OrderBy()
       .SetMaxTop(null)
    );
#endregion
builder.Services.AddCors();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.UseCors(cfg => cfg
.AllowAnyHeader()
.AllowAnyOrigin()
.AllowAnyMethod()
.SetPreflightMaxAge(TimeSpan.FromMinutes(10)));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireRateLimiting("fixed");

//await app.CreateFirstUser();

app.Run();
