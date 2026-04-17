using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services.Auth;
using BFCAI.Nesyan.Application.Abstraction.Services.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Services.Medications;
using BFCAI.Nesyan.Application.Abstraction.Services.MindGames;
using BFCAI.Nesyan.Application.Abstraction.Services.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
using BFCAI.Nesyan.Application.Services.Auth;
using BFCAI.Nesyan.Application.Services.Doctors;
using BFCAI.Nesyan.Application.Services.Medications;
using BFCAI.Nesyan.Application.Services.MindGames;
using BFCAI.Nesyan.Application.Services.Patients;
using BFCAI.Nesyan.Application.Services.TreatmentRequests;
using BFCAI.Nesyan.Domain.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Servicemanager(IUnitOfWork unitOfWork,IMapper mapper,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _doctorService = new Lazy<IDoctorService>(() => new DoctorService(_unitOfWork, _mapper));
            _patientService = new Lazy<IPatientService>(() => new PatientService(_unitOfWork, _mapper));
            _medicationService = new Lazy<IMedicationService>(() => new MedicationService(_unitOfWork, _mapper));
            _treatmentRequestService= new Lazy<ITreatmentRequestService>(()=> new TreatmentRequestService(_unitOfWork, _mapper));
            _authService=new Lazy<IAuthService>(()=>new AuthService(_unitOfWork,_configuration ));
            _mindGamesService = new Lazy<IMindGamesService>(() => new MindGamesService(_unitOfWork,mapper));
        }
        public IDoctorService DoctorService => _doctorService.Value;

        public IPatientService PatientService => _patientService.Value;

        public IMedicationService MedicationService => _medicationService.Value;

        public ITreatmentRequestService TreatmentRequestService => _treatmentRequestService.Value;

        public IMindGamesService MindGamesService => _mindGamesService.Value;

        public IAuthService AuthService => _authService.Value;
    }
}
