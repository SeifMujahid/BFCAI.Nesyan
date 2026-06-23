using BFCAI.Nesyan.Application.Abstraction.Services;
using Hangfire;

namespace BFCAI.Nesyan.APIs.HangfireExtensions
{
    public static class AutomaticDoctorRemovingExtensions
    {
        public static async Task AutomaticDoctorRemoving(this WebApplication app)
        {
            app.UseHangfireDashboard();

           await using var scope =app.Services.CreateAsyncScope();

            var serviceManager =
                scope.ServiceProvider
                    .GetRequiredService<IServiceManager>();

            RecurringJob.AddOrUpdate(
                "doctor-removal-job",

                () => serviceManager
                    .DoctorRemovalBackgroundService
                    .RemoveExpiredDoctors(),

                Cron.Daily());
        }
    }
}
