namespace Mc2.Curd.Core.Domains;

public abstract class Entity<TIdentity> : ValueObject, IEntity<TIdentity> where TIdentity : IIdentity
{
    public TIdentity Id { get; }

    public IIdentity GetIdentity()
    {
        return Id;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}