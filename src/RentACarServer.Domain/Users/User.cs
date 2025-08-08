using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Users.ValueObjects;

namespace RentACarServer.Domain.Users;
public sealed class User : Entity
{
    public User(FirstName firstName, LastName lastName, Email email, UserName userName, Password password)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetEmail(email);
        SetUserName(userName);
        SetPassword(password);
        SetFullName();
        SetForgotPasswordCompleted(new(true));
        SetTFAStatus(new(false));
    }

    private User()
    {
        
    }
    public FirstName FirstName { get; private set; } = default!;
    public LastName LastName { get; private set; } = default!;
    public FullName FullName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public UserName UserName { get; private set; } = default!;
    public Password Password { get; private set; } = default!;
    public ForgotPasswordCode? ForgotPasswordCode { get; private set; }
    public ForgotPasswordCreatedAt? ForgotPasswordCreatedAt { get; private set; }
    public IsForgotPasswordCompleted IsForgotPasswordCompleted { get; private set; } = default!;
    public TFAStatus TFAStatus { get; private set; } = default!;
    public TFACode? TFACode { get; private set; } = default!;
    public TFAConfirmCode? TFAConfirmCode { get; private set; } = default!;
    public TFAExpiresDate? TFAExpiresDate { get; private set; } = default!;
    public TFAIsCompleted? TFAIsCompleted { get; private set; } = default!;


    #region Behaviors
    public bool VerifyPasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(Password.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Password.PasswordHash);
    }
    public void SetFirstName(FirstName firstName)
    {
        FirstName = firstName;
    }
    public void SetLastName(LastName lastName)
    {
        LastName = lastName;
    }
    public void SetFullName()
    {
        FullName = new(FirstName.Value + " " + LastName.Value + " (" + Email.Value + ")");
    }  
    public void SetEmail(Email email)
    {
        Email = email;
    }
    public void SetUserName(UserName userName)
    {
        UserName = userName;
    }
    public void SetPassword(Password password)
    {
        Password = password;
    }
    public void SetForgotPasswordCompleted(IsForgotPasswordCompleted isForgotPasswordCompleted)
    {
        IsForgotPasswordCompleted = isForgotPasswordCompleted;
    }
    public void CreateForgotPasswordId()
    {
        ForgotPasswordCode = new(Guid.CreateVersion7());
        ForgotPasswordCreatedAt = new(DateTimeOffset.Now);
        IsForgotPasswordCompleted = new(false);
    }

    public void SetTFAStatus (TFAStatus tfaStatus)
    {
        TFAStatus = tfaStatus;
    }

    public void CreateTFACode()
    {
        var code = Guid.CreateVersion7().ToString();
        var confirmCode = Guid.CreateVersion7().ToString();
        var expiresDate = DateTimeOffset.Now.AddMinutes(5);
        TFAExpiresDate = new(expiresDate);
        TFAConfirmCode = new(confirmCode);
        TFACode = new(code);
        TFAIsCompleted = new(false);
    }

    public void SetTFACompleted()
    {
        TFAIsCompleted = new(true);
    }

    #endregion

}
