using BFCAI.Nesyan.Application.Abstraction.Models.Assessments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientInfoDto
    {
        public PatientSummaryDto PatientSummary { get; set; } = null!;
        public PatientMedicalDto PatientMedical { get; set; } = null!;
        public AssessmentsToReturnDto? LatestAssessment { get; set; }
    }
}
