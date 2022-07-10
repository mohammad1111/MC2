using Mc2.Curd.Core.Domains;

namespace Mc2.Curd.Core.UOW;

public interface IUow
{
    Task Commit();
}