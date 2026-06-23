using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Repository
{
    internal static class SpecificationEvaluator<TEntity,TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>

    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> query,ISpecification<TEntity, TKey> spec)
        {
            if(spec.Criteria is not null)
            {
                query=query.Where(spec.Criteria);
            }
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            query = spec.IncludeStrings.Aggregate(query,(current, include)=> current.Include(include));


            return query;
        }
    }
}
