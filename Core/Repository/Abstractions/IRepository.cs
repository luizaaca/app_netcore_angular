using Core.Model.Base;

namespace Core.Repository.Abstractions
{
    public interface IRepository<T, TId> : IReadOnlyRepository<T, TId> where T : IAggregateRoot
    {
        void AddAsync(T entity);
        void RemoveAsync(T entity);
        void SaveAsync(T entity);
        void CommitAsync();
    }
}
