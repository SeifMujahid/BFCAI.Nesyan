using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Relatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Caregivers
{
    public class CaregiverPatientRemindersDto
    {
        public CaregiverSummaryDto CaregiverSummary { get; set; } = null!;
        public PatientAppointmentsDto? PatientAppointments { get; set; }
        public PatientRoutineDto? PatientRoutines { get; set; }
        public PatientMedicationsDto? PatientMedications { get; set; }
    }
}
