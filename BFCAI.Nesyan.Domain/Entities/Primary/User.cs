using BFCAI.Nesyan.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Primary
{
    public enum Gender
    {
        Male = 0,
        Female = 1
    }
    public class User : BaseAuditableEntity<int>
    {
        public int NationalId { get; set; }
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Gender Gender { get; set; }
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Age { get; set; }
    }
}
