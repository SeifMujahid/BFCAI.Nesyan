using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Common
{
    public interface IBaseAuditableEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
    public abstract class BaseAuditableEntity<TKey> : BaseEntity<TKey>, IBaseAuditableEntity
        where TKey : IEquatable<TKey>
    {

        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; } = DateTime.UtcNow;

    }
}
