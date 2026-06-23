using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Assessments
{
    public class AssessmentsToReturnDto
    {

        public string RecognitionOfName
        { get; set; } = null!;

        public string RecognitionOfPlace
        { get; set; } = null!;

        public string RecognitionOfTime
        { get; set; } = null!;

        public string AbilityToConcentrate
        { get; set; } = null!;

        public string RecallOfRecentEvents
        { get; set; } = null!;


        // Psychological Status

        public string AnxietyOrStress
        { get; set; } = null!;

        public string DepressionOrSadness
        { get; set; } = null!;

        public string Aggression
        { get; set; } = null!;


        // Daily Activities

        public string EatingAndDrinking
        { get; set; } = null!;

        public string Bathing
        { get; set; } = null!;

        public string Dressing
        { get; set; } = null!;

        public string UsingBathroom
        { get; set; } = null!;

        public string Mobility
        { get; set; } = null!;


        // Notes

        public string? Notes
        { get; set; }
    }
}
