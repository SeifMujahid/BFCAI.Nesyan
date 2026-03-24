using BFCAI.Nesyan.Domain.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Contracts
{
    public interface IUnitOfWork:IAsyncDisposable
    {
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : IEquatable<TKey>;

        Task<int> CompleteAsync();
    }
}
