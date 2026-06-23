using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Infrastructure.Presistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<CustomSaveChangeInterceptor>();

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                       .AddInterceptors(interceptor);
            });
            services.AddScoped(typeof(IStoreContextInitializer), typeof(StoreContextinitializer));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));
            services.AddScoped<CustomSaveChangeInterceptor>();
            return services;
        }

    }
}
