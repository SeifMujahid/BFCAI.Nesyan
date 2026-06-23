using BFCAI.Nesyan.Application.Abstraction.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterPatientAsync(RegisterPatientDto dto);
        Task<AuthResponseDto> RegisterDoctorAsync(RegisterDoctorDto dto);
        Task<AuthResponseDto> RegisterRelativeAsync(RegisterRelativeDto dto);
        Task<AuthResponseDto> RegisterCaregiverAsync(RegisterCaregiverDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> VerifyAccountAsync(VerifyAccountDto dto);
        Task<AuthResponseDto> ResendVerificationCodeAsync(ResendVerificationCodeDto dto);
        Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordDto dto);
    }
}
