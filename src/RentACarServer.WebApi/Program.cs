using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using RentACarServer.Application;
using RentACarServer.Application.Service;
using RentACarServer.Infrastructure;
using RentACarServer.WebApi;
using RentACarServer.WebApi.Modules;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
#region RateLimit
builder.Services.AddRateLimiter(cfr =>
{
    cfr.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.QueueLimit = 100;
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("login-fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.QueueLimit = 1;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("forgot-password-fixed", opt =>
    {
        opt.PermitLimit = 2;
        opt.Window = TimeSpan.FromMinutes(3);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("reset-password-fixed", opt =>
    {
        opt.PermitLimit = 3;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("check-forgot-password-code-fixed", opt =>
    {
        opt.PermitLimit = 2;
        opt.Window = TimeSpan.FromMinutes(1);
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
builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();
builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.UseCors(cfg => cfg
.AllowAnyHeader()
.AllowAnyOrigin()
.AllowAnyMethod()
.SetPreflightMaxAge(TimeSpan.FromMinutes(10)));
app.UseResponseCompression();

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();


app.UseExceptionHandler();

app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();
app.MapAuth();

app.MapGet("/", async (IMailService mailService,CancellationToken cancellationToken) =>
{
    await mailService.SendAsync("emrecan@gmail.com", "Test", "<h1><b>Bu bir test mailidir</b></h1>", cancellationToken);
    return Results.Ok("Mail gönderildi");
});

//await app.CreateFirstUser();

app.Run();
