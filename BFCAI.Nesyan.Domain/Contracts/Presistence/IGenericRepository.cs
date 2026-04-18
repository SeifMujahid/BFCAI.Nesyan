using BFCAI.Nesyan.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Contracts
{
    public interface IGenericRepository<TEntity,TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        IQueryable<TEntity> GetTableNoTracking();
        Task<IEnumerable<TEntity>> GetAllAsync(bool WithTracking = false);
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> spec, bool WithTracking = false);
        Task<TEntity?> Get(TKey id);
        Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, TKey> spec);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
