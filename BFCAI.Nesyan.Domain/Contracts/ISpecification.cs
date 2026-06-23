using BFCAI.Nesyan.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Contracts
{
    public interface ISpecification<TEntity,TKey>
        where TEntity:BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }

        public List<Expression<Func<TEntity, object>>> Includes { get; set; }
        public List<Expression<Func<TEntity, object>>> ThenInclude { get; set; }
        public List<string> IncludeStrings { get; set; }
    }
}
