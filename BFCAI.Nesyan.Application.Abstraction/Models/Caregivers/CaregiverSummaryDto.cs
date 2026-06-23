using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Caregivers
{
    public class CaregiverSummaryDto
    {
        public int CaregiverId { get; set; }
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Age { get; set; }
        public string Phone { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string NationalId { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
