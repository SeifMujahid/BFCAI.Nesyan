using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Infrastructure.Presistence.Data;
using BFCAI.Nesyan.Infrastructure.Presistence.Repository;
using System.Collections.Concurrent;

namespace BFCAI.Nesyan.Infrastructure.Presistence.UnitOfWork
{
    public class UnitOfWork(StoreContext DbContext) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, object> _repositories = new();

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
             where TEntity : BaseEntity<TKey>
             where TKey : IEquatable<TKey>
            => (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, _ => new GenericRepository<TEntity, TKey>(DbContext));
        public Task<int> CompleteAsync() => DbContext.SaveChangesAsync();

        public ValueTask DisposeAsync() => DbContext.DisposeAsync();
    }
}
