using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;

namespace BFCAI.Nesyan.Domain.Entities.Medications
{
    public class Medication : BaseAuditableEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public string Frequency { get; set; } = null!;
        public string ScheduleInstructions { get; set; } = null!;
        public string? Notes { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
    }
}
