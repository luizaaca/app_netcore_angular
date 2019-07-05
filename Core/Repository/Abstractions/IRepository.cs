using Core.Model.Base;
using System.Threading.Tasks;

namespace Core.Repository.Abstractions
{
    public interface IRepository<T, TId> : IReadOnlyRepository<T, TId> where T : IAggregateRoot
    {
        void Add(T entity);
        void Remove(T entity);
        void Save(T entity);
        Task CommitAsync();
    }
}
