using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
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
        Rejected = 0,
        Pending =1,
        Accepted =2,
        Selected = 3,
        RemovalPending = 4
    }
    public class TreatmentRequest:BaseAuditableEntity<int>
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public int RelativeId { get; set; }
        public Relative Relative { get; set; } = null!;
        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; } = null!;
        public int? CaregiverId { get; set; }
        public Caregiver? Caregiver { get; set; } = null!;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public DateTime RequestDate { get; set; }
        public string? Notes { get; set; }
    }
}
