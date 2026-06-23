using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Doctors
{
    public class DoctorPatientDto
    {
        public DoctorSummaryDto DoctorSummary { get; set; } = null!;
        public PatientSummaryDto PatientSummary { get; set; } = null!;

    }
}
