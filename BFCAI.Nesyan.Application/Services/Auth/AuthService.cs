using BFCAI.Nesyan.Application.Abstraction.Services.Auth;
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

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        private async Task<bool> IsEmailOrNationalIdOrUsernameExistsAsync(string email, string nationalId, string userName)
        {
            var patients = await _unitOfWork.GetRepository<Patient, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            var doctors = await _unitOfWork.GetRepository<Doctor, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            var relatives = await _unitOfWork.GetRepository<Relative, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            var caregivers = await _unitOfWork.GetRepository<Caregiver, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            
            return patients || doctors || relatives || caregivers;
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

        public async Task<AuthResponseDto> RegisterPatientAsync(RegisterPatientDto dto)
        {
            if (await IsEmailOrNationalIdOrUsernameExistsAsync(dto.Email, dto.NationalId, dto.UserName))
                return new AuthResponseDto { IsSuccess = false, Message = "Email, National ID, or Username is already registered." };

            var user = new Patient
            {
                Email = dto.Email,
                UserName = dto.UserName,
                NationalId = dto.NationalId,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FName = dto.FName,
                LName = dto.LName,
                Gender = dto.Gender,
                Country = dto.Country,
                City = dto.City,
                Age = dto.Age,
                CurrentStage = dto.CurrentStage,
                Height = dto.Height,
                Weight = dto.Weight,
                BloodType = dto.BloodType,
                ChronicDisease = dto.ChronicDisease,
                CreatedBy = "System", // Default audit property
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = "System",
                LastModifiedOn = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<Patient, int>().AddAsync(user);
            await _unitOfWork.CompleteAsync();

            var token = GenerateJwtToken(user, "Patient");

            return new AuthResponseDto 
            { 
                IsSuccess = true, 
                Message = "Patient registered successfully.",
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                Role = "Patient"
            };
        }

        public async Task<AuthResponseDto> RegisterDoctorAsync(RegisterDoctorDto dto)
        {
            if (await IsEmailOrNationalIdOrUsernameExistsAsync(dto.Email, dto.NationalId, dto.UserName))
                return new AuthResponseDto { IsSuccess = false, Message = "Email, National ID, or Username is already registered." };

            var user = new Doctor
            {
                Email = dto.Email,
                UserName = dto.UserName,
                NationalId = dto.NationalId,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FName = dto.FName,
                LName = dto.LName,
                Gender = dto.Gender,
                Country = dto.Country,
                City = dto.City,
                Age = dto.Age,
                GraduationDegree = SaveFile(dto.GraduationDegree, "doctors/degrees"),
                MedicalAssociationCard = SaveFile(dto.MedicalAssociationCard, "doctors/cards"),
                CreatedBy = "System",
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = "System",
                LastModifiedOn = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<Doctor, int>().AddAsync(user);
            await _unitOfWork.CompleteAsync();

            var token = GenerateJwtToken(user, "Doctor");

            return new AuthResponseDto 
            { 
                IsSuccess = true, 
                Message = "Doctor registered successfully.",
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                Role = "Doctor"
            };
        }

        public async Task<AuthResponseDto> RegisterRelativeAsync(RegisterRelativeDto dto)
        {
            if (await IsEmailOrNationalIdOrUsernameExistsAsync(dto.Email, dto.NationalId, dto.UserName))
                return new AuthResponseDto { IsSuccess = false, Message = "Email, National ID, or Username is already registered." };

            var user = new Relative
            {
                Email = dto.Email,
                UserName = dto.UserName,
                NationalId = dto.NationalId,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FName = dto.FName,
                LName = dto.LName,
                Gender = dto.Gender,
                Country = dto.Country,
                City = dto.City,
                Age = dto.Age,
                CreatedBy = "System",
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = "System",
                LastModifiedOn = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<Relative, int>().AddAsync(user);
            await _unitOfWork.CompleteAsync();

            var token = GenerateJwtToken(user, "Relative");

            return new AuthResponseDto 
            { 
                IsSuccess = true, 
                Message = "Relative registered successfully.",
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                Role = "Relative"
            };
        }

        public async Task<AuthResponseDto> RegisterCaregiverAsync(RegisterCaregiverDto dto)
        {
            if (await IsEmailOrNationalIdOrUsernameExistsAsync(dto.Email, dto.NationalId, dto.UserName))
                return new AuthResponseDto { IsSuccess = false, Message = "Email, National ID, or Username is already registered." };

            var user = new Caregiver
            {
                Email = dto.Email,
                UserName = dto.UserName,
                NationalId = dto.NationalId,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FName = dto.FName,
                LName = dto.LName,
                Gender = dto.Gender,
                Country = dto.Country,
                City = dto.City,
                Age = dto.Age,
                CreatedBy = "System",
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = "System",
                LastModifiedOn = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<Caregiver, int>().AddAsync(user);
            await _unitOfWork.CompleteAsync();

            var token = GenerateJwtToken(user, "Caregiver");

            return new AuthResponseDto 
            { 
                IsSuccess = true, 
                Message = "Caregiver registered successfully.",
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                Role = "Caregiver"
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            User? user = await _unitOfWork.GetRepository<Patient, int>().GetTableNoTracking().FirstOrDefaultAsync(u => u.Email == dto.Email);
            string role = "Patient";

            if (user == null)
            {
                user = await _unitOfWork.GetRepository<Doctor, int>().GetTableNoTracking().FirstOrDefaultAsync(u => u.Email == dto.Email);
                role = "Doctor";
            }
            if (user == null)
            {
                user = await _unitOfWork.GetRepository<Relative, int>().GetTableNoTracking().FirstOrDefaultAsync(u => u.Email == dto.Email);
                role = "Relative";
            }
            if (user == null)
            {
                user = await _unitOfWork.GetRepository<Caregiver, int>().GetTableNoTracking().FirstOrDefaultAsync(u => u.Email == dto.Email);
                role = "Caregiver";
            }

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return new AuthResponseDto { IsSuccess = false, Message = "Invalid email or password." };

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
