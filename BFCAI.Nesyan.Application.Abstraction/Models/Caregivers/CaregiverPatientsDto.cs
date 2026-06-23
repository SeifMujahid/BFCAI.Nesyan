using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Caregivers
{
    public class CaregiverPatientsDto
    {
        public CaregiverSummaryDto CaregiverSummary { get; set; } = null!;
        public IEnumerable<PatientSummaryDto> Patients { get; set; } = null!;
    }
}
