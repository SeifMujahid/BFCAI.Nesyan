using BFCAI.Nesyan.Application.Abstraction.Models.Appointments;
using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientRemindersDto
    {
        public  PatientAppointmentsDto? AppointmentToReturn { get; set; }
        public  PatientRoutineDto? RoutineToReturn { get; set; }
        public  PatientMedicationsDto? PatientMedications { get; set; }
    }
}
