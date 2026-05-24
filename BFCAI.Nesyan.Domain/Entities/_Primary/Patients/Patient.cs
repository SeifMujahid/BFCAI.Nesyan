
using BFCAI.Nesyan.Domain.Entities.Assessments;
using BFCAI.Nesyan.Domain.Entities.IoT;
using BFCAI.Nesyan.Domain.Entities.Medications;
using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Relations;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
namespace BFCAI.Nesyan.Domain.Entities.Primary.Patients
{
    public enum AlzheimerStage
    {
        Stage1_Mild = 1,
        Stage2_Moderate = 2,
        Stage3_Severe = 3
    }
    public enum BloodType
    {
        A_Positive,
        A_Negative,
        B_Positive,
        B_Negative,
        AB_Positive,
        AB_Negative,
        O_Positive,
        O_Negative
    }
    public class Patient : User
    {
        public AlzheimerStage CurrentStage { get; set; } 
        public double Height { get; set; }
        public double Weight { get; set; }
        public BloodType BloodType { get; set; }
        public string ChronicDisease { get; set; } = null!;

        public int? DoctorId { get; set; }
        public int? CaregiverId { get; set; }

        public Doctor? Doctor { get; set; }
        public Caregiver? Caregiver { get; set; }
        public ICollection<Medication> Reminders { get; set; }= new List<Medication>();
        public ICollection<Assessment> Assessments { get; set; }= new List<Assessment>();
        public ICollection<PatientTelemetry> PatientTelemetries { get; set; } = new List<PatientTelemetry>();
        public ICollection<PatientRelative> PatientRelatives { get; set; } = new List<PatientRelative>();
    }
}
