using BFCAI.Nesyan.Domain.Common;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctor;
using BFCAI.Nesyan.Infrastructure.Presistence.Data;
using BFCAI.Nesyan.Infrastructure.Presistence.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.UnitOfWork
{
    public class UnitOfWork(StoreContext DbContext) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, object> _repositories=new();

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
             where TEntity : BaseEntity<TKey>
             where TKey : IEquatable<TKey>
            => (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, _ => new GenericRepository<TEntity, TKey>(DbContext));
        public Task<int> CompleteAsync() => DbContext.SaveChangesAsync();

        public ValueTask DisposeAsync() => DbContext.DisposeAsync();
    }
}
