using System;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientCaregiverDto
    {
        public int Id { get; set; }
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}
