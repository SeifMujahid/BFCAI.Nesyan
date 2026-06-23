using BFCAI.Nesyan.Application.Abstraction.Models._Relations.RelativePatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using BFCAI.Nesyan.Application.Abstraction.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services._Relations
{
    public interface IRelativePatientService
    {
        public Task AddExistingPatient(int relativeId, VerifyPatientDto dto);
        public Task<PatientSummaryV2Dto> RelativeSearchByUserName(string userName);
        public Task CreateRelativePatientRelation(int relativeId, int patientId);
        public Task<RelativePatientsDto> GetRelativePatients(int relativeId);
        public Task<RelativePatientHomeDto> GetPatientHomeAsync(int relativeId, int patientId);
        public  Task<RelativePatientRemindersDto> GetPatientReminders(int relativeId, int patientId, int reminderType);
        public Task UpdateReminder(int relativeId, int patientId, int reminderId, ReminderToUpdateDto dto);
        public Task CreateReminder(int relativeId, int patientId, ReminderToCreateDto dto);

        public Task DeleteReminder(int relativeId, int patientId, int reminderId);

        public Task DeletePatientFromRelative(int relativeId, int patientId);
        public Task<AuthResponseDto> RegisterAndAddPatientAsync(int relativeId, RegisterPatientDto dto);
    }
}
