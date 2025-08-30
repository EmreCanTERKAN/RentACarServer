using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Shared;

namespace RentACarServer.Domain.Categories;
public sealed class Category : Entity
{
    private Category() { }
    public Category(
        Name name,
        bool isActive)
    {
        SetName(name);
        SetStatus(isActive);
    }

    public Name Name { get; set; } = default!;

    #region Behaviors
    public void SetName(Name name)
    {
        Name = name;
    }
    #endregion
}
