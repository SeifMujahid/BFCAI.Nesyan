using BFCAI.Nesyan.Application.Abstraction.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientAppointmentsDto
    {
        public PatientSummaryDto PatientSummary { get; set; } = null!;
        public IEnumerable<AppointmentToReturnDto>? AppointmentToReturn { get; set; }


    }
}
