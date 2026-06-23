using BFCAI.Nesyan.Application.Abstraction.Models.Assessments;
using BFCAI.Nesyan.Application.Abstraction.Models.IoT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientHomeDto
    {
        public PatientSummaryDto Patient { get; set; } = new();

        public TelemetryRequestDto? LatestTelemetry { get; set; }

        public AssessmentsToReturnDto? LatestAssessment { get; set; }
    }
}
