using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Shared;

namespace RentACarServer.Domain.Role;
public sealed class Role : Entity
{
    public Name Name { get; set; } = default!;
}
