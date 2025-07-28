namespace RentACarServer.Domain.Users.ValueObjects;

public sealed record ForgotPasswordCreatedAt(DateTimeOffset CreatedAt)
{
    //public static implicit operator DateTimeOffset(ForgotPasswordCreatedAt createdAt) => createdAt.CreatedAt;
    //public static implicit operator string(ForgotPasswordCreatedAt createdAt) => createdAt.CreatedAt.ToString("o");
};




