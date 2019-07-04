using Core.Infrastructure.Querying;
using Core.Repository.Abstractions;
using Core.Model.Base;
using Microsoft.EntityFrameworkCore;
using Repository.EF.Configuration;
using Repository.EF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF.Repository.Base
{
    public abstract class BaseRepository<T, TEntityKey> : IRepository<T, TEntityKey> where T : EntityBase<TEntityKey>, IAggregateRoot
    {
        protected EFContext context;

        public BaseRepository(EFContext context)
        {
            this.context = context;
        }

        public async void AddAsync(T entity)
            => await context.AddAsync(entity);

        public async void CommitAsync()
            => await context.SaveChangesAsync();

        public async Task<IEnumerable<T>> FindAllAsync()
            => await context.Set<T>().ToListAsync();

        public async Task<T> FindByAsync(TEntityKey id)
            => await context.FindAsync<T>(id);

        public async Task<int> CountAsync(Query<T> query)
            => await query.TranslateIntoEFQuery(context.Set<T>()).CountAsync();

        public async Task<IEnumerable<T>> FindByAsync(Query<T> query)
            => await query.TranslateIntoEFQuery(context.Set<T>()).ToListAsync();

        public async void RemoveAsync(T entity)
        {
            await Task.Run(() =>
            {
                context.Remove(entity);
            });
        }

        public async void SaveAsync(T entity)
        {
            await Task.Run(() =>
            {
                context.Update(entity);
            });
        }
    }
}
