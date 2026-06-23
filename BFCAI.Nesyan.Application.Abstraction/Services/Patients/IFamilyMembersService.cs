using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services.Patients
{
    public interface IFamilyMembersService
    {
        Task<IEnumerable<FamilyMemberDto>> GetFamilyMembersByPatientIdAsync(int patientId);
        Task<FamilyMemberDto> CreateFamilyMemberAsync(FamilyMemberCreateDto dto);
        Task<FamilyMemberDto> UpdateFamilyMemberAsync(int id, FamilyMemberUpdateDto dto);
        Task<bool> DeleteFamilyMemberAsync(int id);
        Task<FamilyMemberDto?> IdentifySpeakerVoiceAsync(int patientId, IFormFile audioFile);
    }
}
