using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services._Relations;
using BFCAI.Nesyan.Application.Abstraction.Services.Auth;
using BFCAI.Nesyan.Application.Abstraction.Services.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Services.IoT;
using BFCAI.Nesyan.Application.Abstraction.Services.Medications;
using BFCAI.Nesyan.Application.Abstraction.Services.MindGames;
using BFCAI.Nesyan.Application.Abstraction.Services.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
using BFCAI.Nesyan.Application.Abstraction.Services.Location;
using BFCAI.Nesyan.Application.Services._Reltaions.RelativePatient;
using BFCAI.Nesyan.Application.Services.Auth;
using BFCAI.Nesyan.Application.Services.Caregivers;
using BFCAI.Nesyan.Application.Services.Doctors;
using BFCAI.Nesyan.Application.Services.IoT;
using BFCAI.Nesyan.Application.Services.Medications;
using BFCAI.Nesyan.Application.Services.MindGames;
using BFCAI.Nesyan.Application.Services.Patients;
using BFCAI.Nesyan.Application.Services.Relatives;
using BFCAI.Nesyan.Application.Services.TreatmentRequests;
using BFCAI.Nesyan.Application.Services.Location;
using BFCAI.Nesyan.Domain.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services
{
    public class Servicemanager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly Lazy<IDoctorService> _doctorService;
        private readonly Lazy<IPatientService> _patientService;
        private readonly Lazy<IMedicationService> _medicationService;
        private readonly Lazy<ITreatmentRequestService> _treatmentRequestService;
        private readonly Lazy<IMindGamesService> _mindGamesService;
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<BFCAI.Nesyan.Application.Abstraction.Services.Relatives.IRelativeService> _relativeService;
        private readonly Lazy<BFCAI.Nesyan.Application.Abstraction.Services.Caregivers.ICaregiverService> _caregiverService;
        private readonly Lazy<ITelemetryService> _telemetryService;
        private readonly Lazy<IRelativePatientService> _relativePatientService;
        private readonly Lazy<IFamilyMembersService> _familyMembersService;
        private readonly Lazy<IDoctorRemovalBackgroundService> _doctorRemovalBackgroundService;
        private readonly Lazy<ILocationService> _locationService;
        private readonly Lazy<IDoctorPatientService> _doctorPatientService;
        private readonly Lazy<ICaregiverPatientService> _caregiverPatientService;

        private readonly IEmailService _emailService;
        private readonly IHttpClientFactory _httpClientFactory;
        public Servicemanager(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IConfiguration configuration, 
            IEmailService emailService, 
            ITelemetryStore telemetryStore,
            IHttpClientFactory httpClientFactory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
            _httpClientFactory = httpClientFactory;
            _doctorService = new Lazy<IDoctorService>(() => new DoctorService(_unitOfWork, _mapper, _httpClientFactory, _configuration));
            _patientService = new Lazy<IPatientService>(() => new PatientService(_unitOfWork, _mapper));
            _medicationService = new Lazy<IMedicationService>(() => new MedicationService(_unitOfWork, _mapper));
            _treatmentRequestService= new Lazy<ITreatmentRequestService>(()=> new TreatmentRequestService(_unitOfWork, _mapper));
            _authService=new Lazy<IAuthService>(()=>new AuthService(_unitOfWork, _configuration, _emailService));
            _mindGamesService = new Lazy<IMindGamesService>(() => new MindGamesService(_unitOfWork, _mapper, _httpClientFactory, _configuration));
            _relativeService = new Lazy<BFCAI.Nesyan.Application.Abstraction.Services.Relatives.IRelativeService>(() => new RelativeService(_unitOfWork, _mapper));
            _caregiverService = new Lazy<BFCAI.Nesyan.Application.Abstraction.Services.Caregivers.ICaregiverService>(() => new CaregiverService(_unitOfWork, _mapper));
            _telemetryService = new Lazy<ITelemetryService>(() => new TelemetryService(telemetryStore, _unitOfWork));
            _relativePatientService = new Lazy<IRelativePatientService>(() => new RelativePatientService(_unitOfWork, _mapper, _emailService));
            _familyMembersService = new Lazy<IFamilyMembersService>(() => new FamilyMembersService(_unitOfWork, _mapper, _httpClientFactory, _configuration));
            _doctorRemovalBackgroundService = new Lazy<IDoctorRemovalBackgroundService>(()=>new DoctorRemovalBackgroundService(_unitOfWork));
            _locationService = new Lazy<ILocationService>(() => new LocationService(_unitOfWork));
            _doctorPatientService = new Lazy<IDoctorPatientService>(() => new BFCAI.Nesyan.Application.Services._Reltaions.DoctorPatient.DoctorPatientService(_unitOfWork, _mapper));
            _caregiverPatientService = new Lazy<ICaregiverPatientService>(() => new BFCAI.Nesyan.Application.Services._Reltaions.CaregiverPatient.CaregiverPatientService(_unitOfWork, _mapper));
        }
        public IDoctorService DoctorService => _doctorService.Value;

        public IPatientService PatientService => _patientService.Value;

        public IMedicationService MedicationService => _medicationService.Value;

        public ITreatmentRequestService TreatmentRequestService => _treatmentRequestService.Value;

        public IMindGamesService MindGamesService => _mindGamesService.Value;

        public IAuthService AuthService => _authService.Value;
        
        public BFCAI.Nesyan.Application.Abstraction.Services.Relatives.IRelativeService RelativeService => _relativeService.Value;
        public BFCAI.Nesyan.Application.Abstraction.Services.Caregivers.ICaregiverService CaregiverService => _caregiverService.Value;
        public ITelemetryService TelemetryService => _telemetryService.Value;

        public IRelativePatientService RelativePatientService => _relativePatientService.Value;

        public IFamilyMembersService FamilyMembersService => _familyMembersService.Value;

        public IDoctorRemovalBackgroundService DoctorRemovalBackgroundService => _doctorRemovalBackgroundService.Value;
        public ILocationService LocationService => _locationService.Value;
        public IDoctorPatientService DoctorPatientService => _doctorPatientService.Value;
        public ICaregiverPatientService CaregiverPatientService => _caregiverPatientService.Value;
    }
}
