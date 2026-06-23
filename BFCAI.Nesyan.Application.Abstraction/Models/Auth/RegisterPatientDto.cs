using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Auth
{
    public class RegisterPatientDto : RegisterUserDto
    {
        public AlzheimerStage CurrentStage { get; set; } = AlzheimerStage.Stage1_Mild; 
        [Required]
        public double Height { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public BloodType BloodType { get; set; }
        
        public List<string> Diseases { get; set; } = new List<string>();
    }
}
