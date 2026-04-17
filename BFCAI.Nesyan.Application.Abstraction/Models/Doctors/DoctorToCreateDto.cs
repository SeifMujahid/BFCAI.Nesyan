using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Doctors
{
    public class DoctorToCreateDto
    {
        public string NationalId { get; set; } = null!;
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Age { get; set; }
        public IFormFile GraduationDegree { get; set; } = null!;
        public IFormFile MedicalAssociationCard { get; set; } = null!;
    }
}
