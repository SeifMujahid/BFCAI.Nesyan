using System.Collections.Generic;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientToCreateDto
    {
        public string NationalId { get; set; } = null!;
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Age { get; set; }
        
        public int CurrentStage { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string BloodType { get; set; } = null!;
        public List<string> Diseases { get; set; } = new List<string>();
        public int? DoctorId { get; set; }
    }
}
