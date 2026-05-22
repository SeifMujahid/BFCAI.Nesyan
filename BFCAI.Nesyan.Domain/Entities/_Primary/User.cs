using BFCAI.Nesyan.Domain.Entities.Common;
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
    public enum MaritalStatus
    {
        Single = 0,
        Married = 1,
        Divorced = 2,
        Widowed = 3,
        Separated = 4
    }
    public class User : BaseAuditableEntity<int>
    {
        public string NationalId { get; set; } = null!;
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string? ImageUrl { get; set; }
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Age { get; set; }
        public bool IsVerified { get; set; } = false;
        public string? VerificationCode { get; set; }
        public DateTime? VerificationCodeExpires { get; set; }
        public string? PasswordResetCode { get; set; }
        public DateTime? PasswordResetCodeExpires { get; set; }
    }
}
