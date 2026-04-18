using System.ComponentModel.DataAnnotations;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Auth
{
    public class RegisterPatientDto : RegisterUserDto
    {
        [Required]
        public AlzheimerStage CurrentStage { get; set; } 
        [Required]
        public double Height { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public BloodType BloodType { get; set; }
        [Required]
        public string ChronicDisease { get; set; } = null!;
    }
}
