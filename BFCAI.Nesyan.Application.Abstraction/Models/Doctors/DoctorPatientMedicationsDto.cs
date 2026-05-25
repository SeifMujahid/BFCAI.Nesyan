using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Doctors
{
    public class DoctorPatientMedicationsDto
    {
        public DoctorSummaryDto DoctorSummary { get; set; } = null!;

        public PatientMedicationsDto PatientMedications { get; set; } = null!;
    }
}
