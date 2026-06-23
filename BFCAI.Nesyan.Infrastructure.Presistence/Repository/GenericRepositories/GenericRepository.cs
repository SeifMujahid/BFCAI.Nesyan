using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Infrastructure.Presistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Repository.GenericRepositories
{
    public class GenericRepository<TEntity, TKey>(StoreContext DbContext) : IGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public IQueryable<TEntity> GetTableNoTracking() => DbContext.Set<TEntity>().AsNoTracking();

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool WithTracking = false)
            => WithTracking ? await DbContext.Set<TEntity>().ToListAsync() :
               await DbContext.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<TEntity?> Get(TKey id)
            =>await DbContext.Set<TEntity>().FindAsync(id);
        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity,TKey>spec,bool WithTracking = false)
            => WithTracking ? await ApplySpecification(spec).ToListAsync() :
               await ApplySpecification(spec).AsNoTracking().ToListAsync();
        public async Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity,TKey> spec)
            => await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task AddAsync(TEntity entity)
            => await DbContext.Set<TEntity>().AddAsync(entity);

        public void Update(TEntity entity)
            => DbContext.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity)
            => DbContext.Set<TEntity>().Remove(entity);

        #region Helpers
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TKey> spec)
            => SpecificationEvaluator<TEntity, TKey>.GetQuery(DbContext.Set<TEntity>().AsQueryable(), spec);        
        #endregion
    }
}
