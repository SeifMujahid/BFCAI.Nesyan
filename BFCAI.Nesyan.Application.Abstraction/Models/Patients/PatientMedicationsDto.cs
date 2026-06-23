using BFCAI.Nesyan.Application.Abstraction.Models.Reminders.Medications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientMedicationsDto
    {
        public PatientSummaryDto PatientSummary { get; set; } = null!;
        public IEnumerable<MedicationToReturnDto>? PatientMedications { get; set; }
    }
}
