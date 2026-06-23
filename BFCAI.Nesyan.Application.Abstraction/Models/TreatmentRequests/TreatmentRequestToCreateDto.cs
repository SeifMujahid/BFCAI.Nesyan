using System.ComponentModel.DataAnnotations;

namespace BFCAI.Nesyan.Application.Abstraction.Models.TreatmentRequests
{
    public class TreatmentRequestToCreateDto
    {
        public int PatientId { get; set; }

        public int? DoctorId { get; set; }
        public int? CaregiverId { get; set; }

        public int RelativeId { get; set; }

        public DateTime? RequestDate { get; set; }

        public string? Notes { get; set; }
    }
}
