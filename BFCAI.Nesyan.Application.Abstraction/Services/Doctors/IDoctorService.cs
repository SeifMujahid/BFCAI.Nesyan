using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services.Doctors
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorSummaryDto>> GetDoctorsAsync();
        Task<IEnumerable<DoctorToReturnDto>> GetDoctorsWithSpecAsync();
        Task<DoctorSummaryDto> GetDoctorAsync(int id);
        Task<DoctorToReturnDto> GetDoctorWithSpecAsync(int id);
        Task<DoctorToReturnDto> CreateDoctorAsync(DoctorToCreateDto doctorToCreate);
        Task UpdateDoctorAsync(DoctorToReturnDto doctorToUpdate);
        Task DeleteDoctorAsync(int id);
        Task<IEnumerable<PatientToReturnDto>> GetDoctorPatientsAsync(int doctorId);
        public Task<DoctorPatientDto> GetDoctorPatientWithSpecAsync(int doctorId, int patientId);
        public Task<DoctorPatientMedicationsDto> GetPatientMedications(int doctorId, int patientId);
        public Task DoctorUpdatePatientStage(int doctorId, int patientId, int stageNumber);
        public  Task CreateReminderByDoctor(int doctorId, int patientId, ReminderToCreateDto dto);
        public  Task UpdateReminderByDoctor(int doctorId, int patientId, int reminderId, ReminderToUpdateDto dto);
        Task<DoctorStatisticsDto> GetDoctorStatisticsAsync(int doctorId);
    }
}
