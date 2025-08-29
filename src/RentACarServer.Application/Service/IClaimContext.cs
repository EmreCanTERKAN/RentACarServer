namespace RentACarServer.Application.Service;
public interface IClaimContext
{
    Guid GetUserId();
    Guid GetBranchId();
    string GetRoleName();
}
