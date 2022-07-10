using Mc2.Curd.Core.Domains;

namespace Mc2.Curd.Core.UniqueConstraints;

public interface IUniqueConstraintService
{
    Task<bool> ExistKey(string key);
    
    Task AddUniqueConstraints(IList<UniqueConstraint> uniqueConstraints);
}