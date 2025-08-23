using Microsoft.AspNetCore.Http;
using RentACarServer.Application.Service;
using System.Security.Claims;

namespace RentACarServer.Infrastructure.Service;
internal sealed class ClaimContext(
    IHttpContextAccessor httpContextAccessor) : IClaimContext
{
    public Guid GetUserId()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            throw new ArgumentNullException("context bilgisi bulunamadı");
        }
        var claims = httpContext.User.Claims;
        string? userId = claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            throw new ArgumentNullException("Kullanıcı bilgisi bulunamadı");
        }
        try
        {
            Guid id = Guid.Parse(userId);
            return id;
        }
        catch (Exception)
        {
            throw new ArgumentException("Kullanıcı id uygun Guid formatında değil");
        }
    }

    public Guid GetBranchId()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            throw new ArgumentNullException("context bilgisi bulunamadı");
        }
        var claims = httpContext.User.Claims;
        string? branchId = claims.FirstOrDefault(i => i.Type == "branchId")?.Value;
        if (branchId is null)
        {
            throw new ArgumentNullException("Şube bilgisi bulunamadı");
        }
        try
        {
            Guid id = Guid.Parse(branchId);
            return id;
        }
        catch (Exception)
        {
            throw new ArgumentException("Şube id uygun Guid formatında değil");
        }
    }
}