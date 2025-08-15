using GenericRepository;
using RentACarServer.Domain.Role;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Roles;
public sealed record RoleDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class RoleDeleteCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RoleDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (role is null)
        {
            return Result<string>.Failure("Bu id'ye ait rol bulunamadı");
        }

        role.Delete();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rol başarıyla silindi";

    }
}
