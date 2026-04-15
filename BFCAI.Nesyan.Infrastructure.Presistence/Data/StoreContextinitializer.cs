using BFCAI.Nesyan.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data
{
    public class StoreContextinitializer(StoreContext DbContext) : IStoreContextInitializer
    {
        public async Task InitalizeAsync()
        {
            var pendingMigrations = await DbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await DbContext.Database.MigrateAsync();
                Console.WriteLine("Database migrated successfully.");
            }
            else
            {
                Console.WriteLine("No pending migrations.");
            }
        }


    }
}
