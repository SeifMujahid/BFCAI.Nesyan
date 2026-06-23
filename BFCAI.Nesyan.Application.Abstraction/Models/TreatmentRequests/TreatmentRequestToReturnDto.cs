using BFCAI.Nesyan.Application.Abstraction.Models.Caregivers;
using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Relatives;

namespace BFCAI.Nesyan.Application.Abstraction.Models.TreatmentRequests
{
    public class TreatmentRequestToReturnDto
    {
        public int requestId { get; set; } 
        public DoctorSummaryDto DoctorSummary { get; set; } = null!;
        public RelativeSummaryDto RelativeSummary { get; set; } = null!;
        public CaregiverSummaryDto CaregiverSummary { get; set; } = null!;
        public PatientInfoDto PatientInfo { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
