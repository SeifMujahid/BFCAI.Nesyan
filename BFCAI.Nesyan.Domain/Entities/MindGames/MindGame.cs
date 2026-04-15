using BFCAI.Nesyan.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.MindGames
{
    public class MindGame:BaseAuditableEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; }= null!;
        public string Brief { get; set; } = null!;
        public string TargetMetrics { get; set; } = null!;
    }
}
