namespace RentACarServer.Domain.Abstractions;
public abstract class Entity
{
    protected Entity()
    {
        Id = new IdentityId(Guid.CreateVersion7());
    }
    public IdentityId Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdateBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }

}

public sealed record IdentityId(Guid Value);