using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentACarServer.Application.Service;
using RentACarServer.Domain.Branches;
using RentACarServer.Domain.LoginTokens;
using RentACarServer.Domain.LoginTokens.ValueObjects;
using RentACarServer.Domain.Roles;
using RentACarServer.Domain.Users;
using RentACarServer.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace RentACarServer.Infrastructure.Service;
internal sealed class JwtProvider(
    ILoginTokenRepository loginTokenRepository,
    IBranchRepository branchRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork,
    IOptions<JwtOptions> options) : IJwtProvider
{
    public async Task<string> CreateTokenAsync(User user, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FirstOrDefaultAsync(p => p.Id == user.RoleId, cancellationToken);
        var branch = await branchRepository.FirstOrDefaultAsync(p => p.Id == user.BranchId, cancellationToken);

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("fullName", user.FirstName.Value + " " + user.LastName.Value),
            new Claim("fullNameWithEmail",user.FullName.Value),
            new Claim("email", user.Email.Value),
            new Claim("role", role?.Name.Value ?? string.Empty),
            new Claim("permissions", role is null ? "" : JsonSerializer.Serialize(role.Permissions.Select(s => s.Value).ToArray())),
            new Claim("branch",branch?.Name.Value ?? string.Empty),
            new Claim("branchId",branch?.Id ?? string.Empty)
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        var expires = DateTime.Now.AddDays(1);

        JwtSecurityToken securityToken = new(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expires,
            signingCredentials: signingCredentials);

        JwtSecurityTokenHandler tokenHandler = new();
        var token = tokenHandler.WriteToken(securityToken);

        Token newToken = new(token);
        ExpiresDate expiresDate = new(expires);
        LoginToken loginToken = new(newToken, user.Id, expiresDate);
        loginTokenRepository.Add(loginToken);

        var loginTokens = await loginTokenRepository
            .Where(p => p.UserId == user.Id && p.IsActive.Value == true)
            .ToListAsync(cancellationToken);

        foreach (var item in loginTokens)
        {
            item.SetIsActive(new(false));
        }
        loginTokenRepository.UpdateRange(loginTokens);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return token;
    }
}
