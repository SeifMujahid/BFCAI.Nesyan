using BFCAI.Nesyan.Application.Abstraction.Models.Caregivers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services.Caregivers
{
    public interface ICaregiverService
    {
        Task<IEnumerable<CaregiverSummaryDto>> GetCaregiversAsync();
        Task<CaregiverPatientsDto> GetCaregiverAsync(int id);
        public  Task<CaregiverPatientHomeDto> GetPatientHome(int caregiverId, int patientId);
        Task<CaregiverToReturnDto> CreateCaregiverAsync(CaregiverToCreateDto caregiverToCreate);
        public Task<CaregiverPatientRemindersDto> GetPatientReminders(int caregiverId, int patientId, int reminderType);
        Task UpdateCaregiverAsync(CaregiverToReturnDto caregiverToUpdate);
        Task DeleteCaregiverAsync(int id);
    }
}
