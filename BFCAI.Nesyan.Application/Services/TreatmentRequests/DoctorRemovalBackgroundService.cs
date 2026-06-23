using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using BFCAI.Nesyan.Domain.Specifications.RequestTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services.TreatmentRequests
{
    public class DoctorRemovalBackgroundService(IUnitOfWork _unitOfWork): IDoctorRemovalBackgroundService
    {
        public async Task RemoveExpiredDoctors()
        {
            var specs = new ExpiredRequestsSpecifications();
            var repo = _unitOfWork.GetRepository<TreatmentRequest, int>();
            var expiredRequests=await repo.GetAllWithSpecAsync(specs);

            foreach (var request in expiredRequests)
            {
                var patientRepo =
                    _unitOfWork
                        .GetRepository<Patient, int>();

                var patient =
                    await patientRepo.Get(request.PatientId);

                if (patient != null)
                {
                    if (request.DoctorId.HasValue)
                        patient.DoctorId = null;

                    else if (request.CaregiverId.HasValue)
                        patient.CaregiverId = null;

                    patientRepo.Update(patient);
                }

                request.Status = RequestStatus.Rejected;

                repo.Update(request);
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}
