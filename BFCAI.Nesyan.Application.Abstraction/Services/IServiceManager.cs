using BFCAI.Nesyan.Application.Abstraction.Services._Relations;
using BFCAI.Nesyan.Application.Abstraction.Services.Auth;
using BFCAI.Nesyan.Application.Abstraction.Services.Caregivers;
using BFCAI.Nesyan.Application.Abstraction.Services.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Services.IoT;
using BFCAI.Nesyan.Application.Abstraction.Services.Medications;
using BFCAI.Nesyan.Application.Abstraction.Services.MindGames;
using BFCAI.Nesyan.Application.Abstraction.Services.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services.Relatives;
using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
using BFCAI.Nesyan.Application.Abstraction.Services.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services
{
    public interface IServiceManager
    {
        public IDoctorService DoctorService { get; }
        public IPatientService PatientService { get; }
        public IMedicationService MedicationService { get; }
        public ITreatmentRequestService TreatmentRequestService { get; }
        public IMindGamesService MindGamesService { get; }
        public IAuthService AuthService { get; }
        public IRelativeService RelativeService { get; }
        public ICaregiverService CaregiverService { get; }
        public ITelemetryService TelemetryService { get; }
        public IRelativePatientService RelativePatientService { get; }
        public IFamilyMembersService FamilyMembersService { get; }
        public IDoctorRemovalBackgroundService DoctorRemovalBackgroundService { get; }
        public ILocationService LocationService { get; }
        public IDoctorPatientService DoctorPatientService { get; }
        public ICaregiverPatientService CaregiverPatientService { get; }
    }
}
