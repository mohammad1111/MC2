namespace Mc2.Curd.Core.Domains;

public interface IEntity
{
    IIdentity GetIdentity();
}

public interface IEntity<out TIdentity> : IEntity
    where TIdentity : IIdentity
{
    TIdentity Id { get; }
}