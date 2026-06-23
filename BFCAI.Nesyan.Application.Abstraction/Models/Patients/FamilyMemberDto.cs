using System;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class FamilyMemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Relation { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? AudioUrl { get; set; }
        public int PatientId { get; set; }
    }
}
