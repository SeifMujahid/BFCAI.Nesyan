using BFCAI.Nesyan.Application.Abstraction.Services.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
using BFCAI.Nesyan.Application.Mapping;
//using BFCAI.Nesyan.Application.Services.Doctors;
//using BFCAI.Nesyan.Application.Services.TreatmentRequests;
using BFCAI.Nesyan.Application.Abstraction.Services.Patients;
using BFCAI.Nesyan.Application.Services.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services.Medications;
//using BFCAI.Nesyan.Application.Services.Medications;
using BFCAI.Nesyan.Application.Abstraction.Services.MindGames;
//using BFCAI.Nesyan.Application.Services.MindGames;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFCAI.Nesyan.Application.Services.Doctors;

namespace BFCAI.Nesyan.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(O =>
            {
                O.AddProfile<MappingProfile>();
            });
            services.AddScoped(typeof(IDoctorService), typeof(DoctorService));
            //services.AddScoped(typeof(ITreatmentRequestService), typeof(TreatmentRequestService));
            services.AddScoped(typeof(IPatientService), typeof(PatientService));
            //services.AddScoped(typeof(IMedicationService), typeof(MedicationService));
            //services.AddScoped(typeof(IMindGamesService), typeof(MindGamesService));
            return services;
        }
    }
}
