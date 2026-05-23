using BFCAI.Nesyan.Application.Abstraction.Services.Auth;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Models.Auth;
using BFCAI.Nesyan.Domain.Entities.Primary;
using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BFCAI.Nesyan.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _emailService = emailService;
        }

        private async Task<bool> IsEmailOrNationalIdOrUsernameExistsAsync(string email, string nationalId, string userName)
        {
            var patients = await _unitOfWork.GetRepository<Patient, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            var doctors = await _unitOfWork.GetRepository<Doctor, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            var relatives = await _unitOfWork.GetRepository<Relative, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            var caregivers = await _unitOfWork.GetRepository<Caregiver, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            
            return patients || doctors || relatives || caregivers;
        }

        private async Task<(User? User, string Role)> GetUserByEmailAsync(string email)
        {
            User? user = await _unitOfWork.GetRepository<Patient, int>().GetTableNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            if (user != null) return (user, "Patient");

            user = await _unitOfWork.GetRepository<Doctor, int>().GetTableNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            if (user != null) return (user, "Doctor");

            user = await _unitOfWork.GetRepository<Relative, int>().GetTableNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            if (user != null) return (user, "Relative");

            user = await _unitOfWork.GetRepository<Caregiver, int>().GetTableNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            if (user != null) return (user, "Caregiver");

            return (null, string.Empty);
        }

        private void UpdateUser(User user, string role)
        {
            if (role == "Patient") _unitOfWork.GetRepository<Patient, int>().Update((Patient)user);
            else if (role == "Doctor") _unitOfWork.GetRepository<Doctor, int>().Update((Doctor)user);
            else if (role == "Relative") _unitOfWork.GetRepository<Relative, int>().Update((Relative)user);
            else if (role == "Caregiver") _unitOfWork.GetRepository<Caregiver, int>().Update((Caregiver)user);
        }

        private string SaveFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0) return string.Empty;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return $"/uploads/{folderName}/{uniqueFileName}";
        }

        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public async Task<AuthResponseDto> RegisterPatientAsync(RegisterPatientDto dto)
        {
            if (await IsEmailOrNationalIdOrUsernameExistsAsync(dto.Email, dto.NationalId, dto.UserName))
                return new AuthResponseDto { IsSuccess = false, Message = "Email, National ID, or Username is already registered." };

            var code = GenerateVerificationCode();
            var user = new Patient
            {
                Email = dto.Email,
                UserName = dto.UserName,
                NationalId = dto.NationalId,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FName = dto.FName,
                LName = dto.LName,
                Phone = dto.Phone,
                Gender = dto.Gender,
                MaritalStatus = dto.MaritalStatus,
                ImageUrl = dto.Image != null ? SaveFile(dto.Image, "patients/avatars") : null,
                Country = dto.Country,
                City = dto.City,
                Age = dto.Age,
                CurrentStage = dto.CurrentStage,
                Height = dto.Height,
                Weight = dto.Weight,
                BloodType = dto.BloodType,
                ChronicDisease = dto.Diseases != null && dto.Diseases.Count > 0 
                    ? string.Join(",", dto.Diseases) 
                    : string.Empty,
                CreatedBy = "System", // Default audit property
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                IsVerified = false,
                VerificationCode = code,
                VerificationCodeExpires = DateTime.UtcNow.AddMinutes(5)
            };

            await _unitOfWork.GetRepository<Patient, int>().AddAsync(user);
            await _unitOfWork.CompleteAsync();

            await _emailService.SendEmailAsync(user.Email, "Verify Your Account", $"Your verification code is: {code}");

            return new AuthResponseDto 
            { 
                IsSuccess = true, 
                Message = "Patient registered successfully. Please check your email for the verification code.",
                Token = string.Empty,
                UserId = user.Id,
                Email = user.Email,
                Role = "Patient"
            };
        }

        public async Task<AuthResponseDto> RegisterDoctorAsync(RegisterDoctorDto dto)
        {
            if (await IsEmailOrNationalIdOrUsernameExistsAsync(dto.Email, dto.NationalId, dto.UserName))
                return new AuthResponseDto { IsSuccess = false, Message = "Email, National ID, or Username is already registered." };

            var code = GenerateVerificationCode();
            var user = new Doctor
            {
                Email = dto.Email,
                UserName = dto.UserName,
                NationalId = dto.NationalId,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FName = dto.FName,
                LName = dto.LName,
                Phone = dto.Phone,
                Gender = dto.Gender,
                MaritalStatus = dto.MaritalStatus,
                ImageUrl = dto.Image != null ? SaveFile(dto.Image, "doctors/avatars") : null,
                Country = dto.Country,
                City = dto.City,
                Age = dto.Age,
                GraduationDegree = SaveFile(dto.GraduationDegree, "doctors/degrees"),
                MedicalAssociationCard = SaveFile(dto.MedicalAssociationCard, "doctors/cards"),
                CreatedBy = "System",
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                IsVerified = false,
                VerificationCode = code,
                VerificationCodeExpires = DateTime.UtcNow.AddMinutes(5)
            };

            await _unitOfWork.GetRepository<Doctor, int>().AddAsync(user);
            await _unitOfWork.CompleteAsync();

            await _emailService.SendEmailAsync(user.Email, "Verify Your Account", $"Your verification code is: {code}");

            return new AuthResponseDto 
            { 
                IsSuccess = true, 
                Message = "Doctor registered successfully. Please check your email for the verification code.",
                Token = string.Empty,
                UserId = user.Id,
                Email = user.Email,
                Role = "Doctor"
            };
        }

        public async Task<AuthResponseDto> RegisterRelativeAsync(RegisterRelativeDto dto)
        {
            if (await IsEmailOrNationalIdOrUsernameExistsAsync(dto.Email, dto.NationalId, dto.UserName))
                return new AuthResponseDto { IsSuccess = false, Message = "Email, National ID, or Username is already registered." };

            var code = GenerateVerificationCode();
            var user = new Relative
            {
                Email = dto.Email,
                UserName = dto.UserName,
                NationalId = dto.NationalId,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FName = dto.FName,
                LName = dto.LName,
                Phone = dto.Phone,
                Gender = dto.Gender,
                MaritalStatus = dto.MaritalStatus,
                ImageUrl = dto.Image != null ? SaveFile(dto.Image, "relatives/avatars") : null,
                Country = dto.Country,
                City = dto.City,
                Age = dto.Age,
                CreatedBy = "System",
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                IsVerified = false,
                VerificationCode = code,
                VerificationCodeExpires = DateTime.UtcNow.AddMinutes(5)
            };

            await _unitOfWork.GetRepository<Relative, int>().AddAsync(user);
            await _unitOfWork.CompleteAsync();

            await _emailService.SendEmailAsync(user.Email, "Verify Your Account", $"Your verification code is: {code}");

            return new AuthResponseDto 
            { 
                IsSuccess = true, 
                Message = "Relative registered successfully. Please check your email for the verification code.",
                Token = string.Empty,
                UserId = user.Id,
                Email = user.Email,
                Role = "Relative"
            };
        }

        public async Task<AuthResponseDto> RegisterCaregiverAsync(RegisterCaregiverDto dto)
        {
            if (await IsEmailOrNationalIdOrUsernameExistsAsync(dto.Email, dto.NationalId, dto.UserName))
                return new AuthResponseDto { IsSuccess = false, Message = "Email, National ID, or Username is already registered." };

            var code = GenerateVerificationCode();
            var user = new Caregiver
            {
                Email = dto.Email,
                UserName = dto.UserName,
                NationalId = dto.NationalId,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FName = dto.FName,
                LName = dto.LName,
                Phone = dto.Phone,
                Gender = dto.Gender,
                MaritalStatus = dto.MaritalStatus,
                ImageUrl = dto.Image != null ? SaveFile(dto.Image, "caregivers/avatars") : null,
                Country = dto.Country,
                City = dto.City,
                Age = dto.Age,
                CreatedBy = "System",
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                IsVerified = false,
                VerificationCode = code,
                VerificationCodeExpires = DateTime.UtcNow.AddMinutes(5)
            };

            await _unitOfWork.GetRepository<Caregiver, int>().AddAsync(user);
            await _unitOfWork.CompleteAsync();

            await _emailService.SendEmailAsync(user.Email, "Verify Your Account", $"Your verification code is: {code}");

            return new AuthResponseDto 
            { 
                IsSuccess = true, 
                Message = "Caregiver registered successfully. Please check your email for the verification code.",
                Token = string.Empty,
                UserId = user.Id,
                Email = user.Email,
                Role = "Caregiver"
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var (user, role) = await GetUserByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return new AuthResponseDto { IsSuccess = false, Message = "Invalid email or password." };

            if (!user.IsVerified)
                return new AuthResponseDto { IsSuccess = false, Message = "Account is not verified. Please verify your account first." };

            var token = GenerateJwtToken(user, role);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Login successful.",
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                Role = role
            };
        }

        public async Task<AuthResponseDto> VerifyAccountAsync(VerifyAccountDto dto)
        {
            var (user, role) = await GetUserByEmailAsync(dto.Email);

            if (user == null)
                return new AuthResponseDto { IsSuccess = false, Message = "User not found." };

            if (user.IsVerified)
                return new AuthResponseDto { IsSuccess = false, Message = "Account is already verified." };

            if (user.VerificationCode != dto.Code || user.VerificationCodeExpires < DateTime.UtcNow)
                return new AuthResponseDto { IsSuccess = false, Message = "Invalid or expired verification code." };

            user.IsVerified = true;
            user.VerificationCode = null;
            user.VerificationCodeExpires = null;

            UpdateUser(user, role);
            await _unitOfWork.CompleteAsync();

            return new AuthResponseDto { IsSuccess = true, Message = "Account verified successfully." };
        }

        public async Task<AuthResponseDto> ResendVerificationCodeAsync(ResendVerificationCodeDto dto)
        {
            var (user, role) = await GetUserByEmailAsync(dto.Email);

            if (user == null)
                return new AuthResponseDto { IsSuccess = false, Message = "User not found." };

            if (user.IsVerified)
                return new AuthResponseDto { IsSuccess = false, Message = "Account is already verified." };

            var code = GenerateVerificationCode();
            user.VerificationCode = code;
            user.VerificationCodeExpires = DateTime.UtcNow.AddMinutes(5);

            UpdateUser(user, role);
            await _unitOfWork.CompleteAsync();

            await _emailService.SendEmailAsync(user.Email, "Verify Your Account", $"Your verification code is: {code}");

            return new AuthResponseDto { IsSuccess = true, Message = "A new verification code has been sent to your email." };
        }

        public async Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var (user, role) = await GetUserByEmailAsync(dto.Email);

            if (user == null)
                return new AuthResponseDto { IsSuccess = false, Message = "User not found." };

            var code = GenerateVerificationCode();
            user.PasswordResetCode = code;
            user.PasswordResetCodeExpires = DateTime.UtcNow.AddMinutes(5);

            UpdateUser(user, role);
            await _unitOfWork.CompleteAsync();

            await _emailService.SendEmailAsync(user.Email, "Reset Your Password", $"Your password reset code is: {code}");

            return new AuthResponseDto { IsSuccess = true, Message = "Password reset code sent to your email." };
        }

        public async Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var (user, role) = await GetUserByEmailAsync(dto.Email);

            if (user == null)
                return new AuthResponseDto { IsSuccess = false, Message = "User not found." };

            if (user.PasswordResetCode != dto.Code || user.PasswordResetCodeExpires < DateTime.UtcNow)
                return new AuthResponseDto { IsSuccess = false, Message = "Invalid or expired password reset code." };

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetCode = null;
            user.PasswordResetCodeExpires = null;

            UpdateUser(user, role);
            await _unitOfWork.CompleteAsync();

            return new AuthResponseDto { IsSuccess = true, Message = "Password reset successfully." };
        }

        private string GenerateJwtToken(User user, string role)
        {
            var jwtSettingsSection = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettingsSection["AccessKey"] ?? throw new InvalidOperationException("Jwt access key is empty"));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // No Expires property, as requested: no expire date to the token
                SigningCredentials = creds,
                Issuer = jwtSettingsSection["Issuer"],
                Audience = jwtSettingsSection["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
