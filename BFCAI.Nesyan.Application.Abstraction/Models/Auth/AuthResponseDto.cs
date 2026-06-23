namespace BFCAI.Nesyan.Application.Abstraction.Models.Auth
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public int? UserId { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Stage { get; set; }
    }
}
