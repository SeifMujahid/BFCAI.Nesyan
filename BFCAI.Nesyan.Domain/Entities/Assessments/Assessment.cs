using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Assessments
{
    public enum AssessmentLevel
    {
        Poor = 1,
        Moderate = 2,
        Good = 3
    }

    public enum PsychologicalLevel
    {
        No = 1,
        Sometimes = 2,
        Yes = 3
    }

    public enum IndependenceLevel
    {
        Dependent = 1,
        NeedsAssistance = 2,
        Independent = 3
    }

    public class Assessment : BaseAuditableEntity<int>
    {
        // Cognitive Assessment
        public AssessmentLevel RecognitionOfName { get; set; }

        public AssessmentLevel RecognitionOfPlace { get; set; }

        public AssessmentLevel RecognitionOfTime { get; set; }

        public AssessmentLevel AbilityToConcentrate { get; set; }

        public AssessmentLevel RecallOfRecentEvents { get; set; }

        // Psychological Status
        public PsychologicalLevel AnxietyOrStress { get; set; }

        public PsychologicalLevel DepressionOrSadness { get; set; }

        public PsychologicalLevel Aggression { get; set; }

        // Daily Activities
        public IndependenceLevel EatingAndDrinking { get; set; }

        public IndependenceLevel Bathing { get; set; }

        public IndependenceLevel Dressing { get; set; }

        public IndependenceLevel UsingBathroom { get; set; }

        public IndependenceLevel Mobility { get; set; }

        // Notes
        public string? Notes { get; set; }

        // Relations
        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;


    }
}
