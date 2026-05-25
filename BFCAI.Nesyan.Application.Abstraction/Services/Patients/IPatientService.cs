using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;

namespace BFCAI.Nesyan.Application.Abstraction.Services.Patients
{
    public interface IPatientService
    {
        Task UpdatePatientStageAsync(int patientId, int newStage);
        Task<PatientHomeDto> GetPatientHome(int patientId);
        Task<PatientFullProfileDto> GetPatientProfileAsync(int patientId);
        Task<PatientRemindersDto> GetPatientReminder(int id, int reminderType);
        Task<IEnumerable<PatientSummaryDto>> GetPatientsAsync();
        Task<PatientToReturnDto> CreatePatientAsync(PatientToCreateDto patientToCreate);
        Task UpdatePatientAsync(PatientToUpdateDto patientToUpdate);
        Task DeletePatientAsync(int id);
        Task CreateReminderAsync(int patientId, ReminderToCreateDto dto);
        Task UpdateReminderAsync(int patientId, int reminderId, ReminderToUpdateDto dto);
        Task DeleteReminderAsync(int patientId, int reminderId);
    }
}
