using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Reports
{
    public class Report:BaseAuditableEntity<int>
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
        public string TargetMetrics { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
