using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services.Doctors
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorToReturnDto>> GetDoctorsAsync();
        Task<DoctorToReturnDto> GetDoctorAsync(int id);
        Task<DoctorToReturnDto> CreateDoctorAsync(DoctorToCreateDto doctorToCreate);
        Task UpdateDoctorAsync(DoctorToReturnDto doctorToUpdate);
        Task DeleteDoctorAsync(int id);
        Task<IEnumerable<PatientToReturnDto>> GetDoctorPatientsAsync(int doctorId);
        Task<DoctorStatisticsDto> GetDoctorStatisticsAsync(int doctorId);
    }
}
