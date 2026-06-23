using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientMedicalDto
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public BloodType BloodType { get; set; }
        public string ChronicDisease { get; set; } = null!;
    }
}
