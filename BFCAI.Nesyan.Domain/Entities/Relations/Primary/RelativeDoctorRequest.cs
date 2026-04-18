using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Relations.Primary
{
    public enum RequestStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2,
        Selected = 3
    }
    public class RelativeDoctorRequest:BaseAuditableEntity<int>
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public int RelativeId { get; set; }
        public Relative Relative { get; set; } = null!;
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public string? Notes { get; set; }
    }
}
