using Microsoft.AspNetCore.Http;
using System;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class FamilyMemberUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Relation { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int PatientId { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? Audio { get; set; }
    }
}
