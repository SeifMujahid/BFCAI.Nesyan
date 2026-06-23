
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services.Medications
{
    public interface IMedicationService
    {
        Task<IEnumerable<RoutineToReturnDto>> GetPatientMedicationsAsync(int patientId);
        Task<RoutineToReturnDto> AddMedicationAsync(ReminderToCreateDto dto);
        Task DeleteMedicationAsync(int id);
    }
}
