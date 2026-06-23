using System.ComponentModel.DataAnnotations;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Auth
{
    public class ResendVerificationCodeDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
