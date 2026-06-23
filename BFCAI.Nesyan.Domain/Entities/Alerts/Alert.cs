using BFCAI.Nesyan.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Alerts
{
    public class Alert:BaseAuditableEntity<int>
    {
        public string Title { get; set; } = null!; 
        public string Category { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string Message { get; set; } = null!;

    }
}
