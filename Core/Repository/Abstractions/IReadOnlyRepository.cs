using Core.Infrastructure.Querying;
using Core.Model.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repository.Abstractions
{
    public interface IReadOnlyRepository<T, TEntityKey> where T : IAggregateRoot
    {
        Task<int> CountAsync(Query<T> query);
        Task<T> FindByAsync(TEntityKey id);
        Task<IEnumerable<T>> FindAllAsync();
        Task<IEnumerable<T>> FindByAsync(Query<T> query);
    }
}
