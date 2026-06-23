using BFCAI.Nesyan.Domain.Entities.Common;
using System;

namespace BFCAI.Nesyan.Domain.Entities.Primary.Patients
{
    public class FamilyMember : BaseAuditableEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Relation { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? AudioUrl { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
    }
}
