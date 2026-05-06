using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services.Auth;
using BFCAI.Nesyan.Application.Abstraction.Services.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Services.Medications;
using BFCAI.Nesyan.Application.Abstraction.Services.MindGames;
using BFCAI.Nesyan.Application.Abstraction.Services.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
using BFCAI.Nesyan.Application.Mapping;
using BFCAI.Nesyan.Application.Services;
using BFCAI.Nesyan.Application.Services.Auth;
using BFCAI.Nesyan.Application.Services.Doctors;
using BFCAI.Nesyan.Application.Services.Medications;
using BFCAI.Nesyan.Application.Services.MindGames;
using BFCAI.Nesyan.Application.Services.Patients;
using BFCAI.Nesyan.Application.Services.TreatmentRequests;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BFCAI.Nesyan.Application.Services.CurrentUserService;

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
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<BFCAI.Nesyan.Application.Abstraction.Services.IoT.ITelemetryStore, BFCAI.Nesyan.Application.Services.IoT.TelemetryStore>();
            return services;
        }
    }
}
