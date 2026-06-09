using BFCAI.Nesyan.Application.Abstraction.Models._Relations.CaregiverPatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services._Relations
{
    public interface ICaregiverPatientService
    {
        Task AddPatientToCaregiverAsync(CaregiverPatientAddDto dto);
        Task<IEnumerable<PatientSummaryDto>> GetCaregiverPatientsAsync(int caregiverId);
        Task<PatientCaregiverDto> GetPatientCaregiverAsync(int patientId);
        Task UpdatePatientCaregiverAsync(int patientId, int caregiverId);
        Task RemovePatientFromCaregiverAsync(int caregiverId, int patientId);
    }
}
