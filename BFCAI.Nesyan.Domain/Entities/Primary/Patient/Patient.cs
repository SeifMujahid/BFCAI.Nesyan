
using BFCAI.Nesyan.Domain.Entities.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
namespace BFCAI.Nesyan.Domain.Entities.Primary.Patient
{
    public enum AlzheimerStage
    {
        Stage1_Mild = 1,
        Stage2_Moderate = 2,
        Stage3_Severe = 3
    }
    public enum BloodType
    {
        A_Positive,
        A_Negative,
        B_Positive,
        B_Negative,
        AB_Positive,
        AB_Negative,
        O_Positive,
        O_Negative
    }
    public class Patient : User
    {
        public AlzheimerStage CurrentStage { get; set; } 
        public double Height { get; set; }
        public double Weight { get; set; }
        public BloodType BloodType { get; set; }
        public string ChronicDisease { get; set; } = null!;


    }
}
