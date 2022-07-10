using Mc2.Curd.Core.Domains;

namespace Mc2.Curd.Core.Repositories;

public interface IUniqueConstraintRepository
{
    Task<string> GetByAsync(string key);
    
    Task<bool> ExistKey(string key);
    
    Task AddUniqueConstraints(IList<UniqueConstraint> uniqueConstraints);
}