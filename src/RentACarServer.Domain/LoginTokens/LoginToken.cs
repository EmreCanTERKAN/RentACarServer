﻿using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.LoginTokens.ValueObjects;

namespace RentACarServer.Domain.LoginTokens;
public sealed class LoginToken
{
    private LoginToken() { }
    public LoginToken(
        Token token,
        IdentityId userId,
        ExpiresDate expiresDate)
    {
        Id = new(Guid.CreateVersion7());
        SetIsActive(new(true));
        SetToken(token);
        SetUserId(userId);
        SetExpiresDate(expiresDate);
    }

    public IdentityId Id { get; private set; } = default!;
    public Token Token { get; private set; } = default!;
    public IdentityId UserId { get; private set; } = default!;
    public IsActive IsActive { get; private set; } = default!;
    public ExpiresDate ExpiresDate { get; private set; } = default!;

    #region Behaviors
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

    #endregion

}
