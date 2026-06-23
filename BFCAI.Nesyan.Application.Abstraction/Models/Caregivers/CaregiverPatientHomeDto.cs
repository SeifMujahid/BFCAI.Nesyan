using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Relatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Caregivers
{
    public class CaregiverPatientHomeDto
    {
        public CaregiverSummaryDto CaregiverSummary { get; set; } = new();
        public PatientHomeDto Patient { get; set; } = new();
    }
}
