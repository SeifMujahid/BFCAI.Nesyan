using System;
using System.Collections.Generic;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientToReturnDto
    {
        public int Id { get; set; }
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public int CurrentStage { get; set; }
        public string CurrentStageName { get; set; } = null!;
        public double Height { get; set; }
        public double Weight { get; set; }
        public string BloodType { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int? DoctorId { get; set; }
        public int? CaregiverId { get; set; }
        public List<string> Diseases { get; set; } = new List<string>();
    }
}
