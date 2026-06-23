using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientRoutineDto
    {
        public PatientSummaryDto PatientSummary { get; set; } = null!;
        public IEnumerable<RoutineToReturnDto>? RoutineToReturn { get; set; }
    }
}
