using Microsoft.AspNetCore.Http;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Caregivers
{
    public class CaregiverToReturnDto
    {
        public int Id { get; set; }
        public string NationalId { get; set; } = null!;
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Gender { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Age { get; set; }
        public IFormFile? Image { get; set; }
    }
}
