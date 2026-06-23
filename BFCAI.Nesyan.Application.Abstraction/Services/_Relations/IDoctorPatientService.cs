using BFCAI.Nesyan.Application.Abstraction.Models._Relations.DoctorPatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services._Relations
{
    public interface IDoctorPatientService
    {
        Task AddPatientToDoctorAsync(DoctorPatientAddDto dto);
        Task<IEnumerable<PatientSummaryDto>> GetDoctorPatientsAsync(int doctorId);
        Task<PatientDoctorDto> GetPatientDoctorAsync(int patientId);
        Task UpdatePatientDoctorAsync(int patientId, int doctorId);
        Task RemovePatientFromDoctorAsync(int doctorId, int patientId);
    }
}
