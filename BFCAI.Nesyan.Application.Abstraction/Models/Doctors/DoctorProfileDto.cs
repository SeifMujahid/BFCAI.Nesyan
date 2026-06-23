using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using System.Collections.Generic;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Doctors
{
    public class DoctorProfileDto
    {
        public int Id { get; set; }
        public string NationalId { get; set; } = null!;
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Gender { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Age { get; set; }
        public string GraduationDegree { get; set; } = null!;
        public string MedicalAssociationCard { get; set; } = null!;
        public string Specialization { get; set; } = null!;
        public IEnumerable<PatientSummaryDto> Patients { get; set; } = new List<PatientSummaryDto>();
    }
}
