using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Branchs.ValueObjects;

namespace RentACarServer.Domain.Branchs;
public sealed class Branch : Entity
{
    private Branch()
    {
        
    }
    public Branch(
        Name name,
        Address address)
    {
        SetName(name);
        SetAddress(address);
    }  
    public Name Name { get; private set; } = default!;
    public Address Address { get; private set; } = default!;

    #region Behaviors
    public void SetName(Name name)
    {
        Name = name;
    }

    public void SetAddress(Address address)
    {
        Address = address;
    }

    #endregion
}
