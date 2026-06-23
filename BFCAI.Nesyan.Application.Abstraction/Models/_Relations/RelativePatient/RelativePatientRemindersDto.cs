using BFCAI.Nesyan.Application.Abstraction.Models.Appointments;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Relatives;
using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models._Relations.RelativePatient
{
    public class RelativePatientRemindersDto
    {
        public RelativeSummaryDto RelativeSummary { get; set; } = null!;
        public PatientAppointmentsDto? PatientAppointments { get; set; }
        public PatientRoutineDto? PatientRoutines { get; set; }
        public PatientMedicationsDto? PatientMedications { get; set;}

    }
}
