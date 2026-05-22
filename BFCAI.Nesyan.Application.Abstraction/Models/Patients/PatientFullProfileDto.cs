using BFCAI.Nesyan.Application.Abstraction.Models.Appointments;
using BFCAI.Nesyan.Application.Abstraction.Models.Assessments;
using BFCAI.Nesyan.Application.Abstraction.Models.IoT;
using BFCAI.Nesyan.Application.Abstraction.Models.MindGames;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders.Medications;
using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using System.Collections.Generic;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientFullProfileDto
    {
        public int Id { get; set; }
        public string NationalId { get; set; } = null!;
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public int CurrentStage { get; set; }
        public string CurrentStageName { get; set; } = null!;
        public double Height { get; set; }
        public double Weight { get; set; }
        public string BloodType { get; set; } = null!;
        public List<string> Diseases { get; set; } = new List<string>();

        public IEnumerable<TelemetryRequestDto> Telemetries { get; set; } = new List<TelemetryRequestDto>();
        public IEnumerable<AssessmentsToReturnDto> Assessments { get; set; } = new List<AssessmentsToReturnDto>();
        public IEnumerable<MedicationToReturnDto> Medications { get; set; } = new List<MedicationToReturnDto>();
        public IEnumerable<AppointmentToReturnDto> Appointments { get; set; } = new List<AppointmentToReturnDto>();
        public IEnumerable<RoutineToReturnDto> Routines { get; set; } = new List<RoutineToReturnDto>();
        public IEnumerable<PatientMindGameDto> AssignedGames { get; set; } = new List<PatientMindGameDto>();
    }
}
