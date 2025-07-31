using RentACarServer.Domain.Abstractions;

namespace RentACarServer.Domain.LoginTokens;
public sealed class LoginToken
{
    public LoginToken(IsActive isActive, Token token, IdentityId userId, ExpiresDate expiresDate)
    {
        Id = new(Guid.CreateVersion7());
        SetIsActive(isActive);
        SetToken(token);
        SetUserId(userId);
        SetExpiresDate(expiresDate);
    }

    public IdentityId Id { get; private set; }
    public IsActive IsActive { get; private set; } = default!;
    public Token Token { get; private set; } = default!;
    public IdentityId UserId { get; private set; } = default!;
    public ExpiresDate ExpiresDate { get; private set; } = default!;

    public void SetIsActive(IsActive isActive)
    {
        IsActive = isActive;
    }

    public void SetToken(Token token)
    {
        Token = token;
    }

    public void SetUserId(IdentityId userId)
    {
        UserId = userId;
    }

    public void SetExpiresDate(ExpiresDate expiresDate)
    {
        ExpiresDate = expiresDate;
    }


}


public sealed record IsActive(bool Value);
public sealed record Token(string Value);
public sealed record ExpiresDate(DateTimeOffset Value);