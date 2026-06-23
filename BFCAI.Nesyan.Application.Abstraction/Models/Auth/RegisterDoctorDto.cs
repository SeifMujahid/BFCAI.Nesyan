using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Auth
{
    public class RegisterDoctorDto : RegisterUserDto
    {
        [Required]
        public string Specialization { get; set; } = null!;
        [Required]
        public IFormFile GraduationDegree { get; set; } = null!;
        [Required]
        public IFormFile MedicalAssociationCard { get; set; } = null!;

    }
}
