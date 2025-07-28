namespace RentACarServer.Application.Service;
public interface IMailService
{
    Task SendAsync(string to, string subject, string body,CancellationToken cancellationToken = default);
}
