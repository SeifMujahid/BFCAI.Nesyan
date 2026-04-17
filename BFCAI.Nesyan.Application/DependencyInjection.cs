using BFCAI.Nesyan.Application.Abstraction.Services.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
using BFCAI.Nesyan.Application.Mapping;
using BFCAI.Nesyan.Application.Abstraction.Services.Patients;
using BFCAI.Nesyan.Application.Services.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services.Medications;
using BFCAI.Nesyan.Application.Services.Medications;
using BFCAI.Nesyan.Application.Abstraction.Services.MindGames;
using BFCAI.Nesyan.Application.Services.MindGames;
using BFCAI.Nesyan.Application.Abstraction.Services.Auth;
using BFCAI.Nesyan.Application.Services.Auth;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFCAI.Nesyan.Application.Services.Doctors;
using BFCAI.Nesyan.Application.Services.TreatmentRequests;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Services;

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
            services.AddScoped(typeof(IServiceManager), typeof(Servicemanager));
            return services;
        }
    }
}
