﻿namespace RentACarServer.Infrastructure.Options;
public sealed class MailSettingOptions
{
    public string Email { get; set; } = default!;
    public string Smtp { get; set; } = default!;
    public int Port { get; set; } = default!;
    public string SSL { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public string Password { get; set; } = default!;
}
