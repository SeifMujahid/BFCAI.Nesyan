using BFCAI.Nesyan.Domain.Contracts;

namespace BFCAI.Nesyan.APIs.Extensions
{
    internal static class InitializerExtension
    {
        public static async Task<WebApplication> InitializerStoreContextAsync(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var environment = services.GetRequiredService<IHostEnvironment>();
            var storeContextInitializer = services.GetRequiredService<IStoreContextInitializer>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await storeContextInitializer.InitalizeAsync();
                await storeContextInitializer.SeedAsync();

            }
            catch (Exception ex)
            {
                if (environment.IsProduction())
                {
                    Console.WriteLine("an error occurred please try again later");
                }
                else
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred while initializing the database.");
                }
            }
            return app;
        }
        }
    }
