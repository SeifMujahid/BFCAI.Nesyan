using BFCAI.Nesyan.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using BFCAI.Nesyan.Domain.Entities.Alerts;
using BFCAI.Nesyan.Domain.Entities.MindGames;
using BFCAI.Nesyan.Domain.Entities.Reports;
using BFCAI.Nesyan.Domain.Entities.Medications;
using Microsoft.Data.SqlClient;
using BFCAI.Nesyan.Domain.Entities.IoT;
using BFCAI.Nesyan.Domain.Entities.Assessments;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data
{
    public class StoreContextinitializer(StoreContext DbContext) : IStoreContextInitializer
    {
        public async Task InitalizeAsync()
        {
            var db = DbContext.Database;
            var hasHistory = false;
            var hasExistingTables = false;

            var originalConnectionString = db.GetConnectionString();

            var builder = new SqlConnectionStringBuilder(originalConnectionString)
            {
                InitialCatalog = "master"
            };

            await using var conn = new SqlConnection(builder.ConnectionString);

            await conn.OpenAsync();

            try
            {
                // Check if target database exists
                using (var dbCmd = conn.CreateCommand())
                {
                    dbCmd.CommandText =
                        "SELECT COUNT(*) FROM sys.databases WHERE name = 'BFCAI.Nesyan.APIs'";

                    var exists = (int)await dbCmd.ExecuteScalarAsync()! > 0;

                    if (!exists)
                    {
                        await conn.CloseAsync();

                        await DbContext.Database.MigrateAsync();

                        Console.WriteLine("Database created and migrated successfully.");
                        return;
                    }
                }

                // Switch to target DB
                conn.ChangeDatabase("BFCAI.Nesyan.APIs");

                bool TableExists(string tableName)
                {
                    using var cmd = conn.CreateCommand();

                    cmd.CommandText = "SELECT OBJECT_ID(@p0);";

                    var p = cmd.CreateParameter();
                    p.ParameterName = "@p0";
                    p.Value = $"[dbo].[{tableName}]";

                    cmd.Parameters.Add(p);

                    var result = cmd.ExecuteScalar();

                    return result != null && result != DBNull.Value;
                }

                bool HasMigrationHistoryRows()
                {
                    if (!TableExists("__EFMigrationsHistory"))
                        return false;

                    using var cmd = conn.CreateCommand();

                    cmd.CommandText =
                        "SELECT TOP(1) 1 FROM [__EFMigrationsHistory];";

                    var result = cmd.ExecuteScalar();

                    return result != null && result != DBNull.Value;
                }

                hasHistory = HasMigrationHistoryRows();
                hasExistingTables = TableExists("Alerts");

                if (!hasHistory && hasExistingTables)
                {
                    Console.WriteLine(
                        "Database migration skipped: schema exists but migrations history is missing.");

                    return;
                }
            }
            finally
            {
                await conn.CloseAsync();
            }

            var pendingMigrations =
                await DbContext.Database.GetPendingMigrationsAsync();

            if (!pendingMigrations.Any())
            {
                Console.WriteLine("No pending migrations.");
                return;
            }

            await DbContext.Database.MigrateAsync();

            Console.WriteLine("Database migrated successfully.");
        }

        public async Task SeedAsync()
        {
            try
            {
                if (!await DbContext.Doctors.AnyAsync())
                {
                    var doctorsData = await File.ReadAllTextAsync("../BFCAI.Nesyan.Infrastructure.Presistence/_Data/Seeds/doctors.json");
                    var doctors = JsonSerializer.Deserialize<List<Doctor>>(doctorsData);

                    if (doctors?.Count > 0)
                    {
                        await DbContext.Set<Doctor>().AddRangeAsync(doctors);
                        await DbContext.SaveChangesAsync();
                    }
                }

                
                if (!await DbContext.Caregivers.AnyAsync())
                {
                    var caregiversData = await File.ReadAllTextAsync("../BFCAI.Nesyan.Infrastructure.Presistence/_Data/Seeds/caregivers.json");
                    var caregivers = JsonSerializer.Deserialize<List<Caregiver>>(caregiversData);

                    if (caregivers?.Count > 0)
                    {
                        await DbContext.Set<Caregiver>().AddRangeAsync(caregivers);
                        await DbContext.SaveChangesAsync();
                    }
                }

                if (!await DbContext.Patients.AnyAsync())
                {
                    var patientsData = await File.ReadAllTextAsync("../BFCAI.Nesyan.Infrastructure.Presistence/_Data/Seeds/patients.json");
                    var patients = JsonSerializer.Deserialize<List<Patient>>(patientsData);

                    if (patients?.Count > 0)
                    {
                        await DbContext.Set<Patient>().AddRangeAsync(patients);
                        await DbContext.SaveChangesAsync();
                    }
                }

                if (!await DbContext.Relatives.AnyAsync())
                {
                    var relativesData = await File.ReadAllTextAsync("../BFCAI.Nesyan.Infrastructure.Presistence/_Data/Seeds/relatives.json");
                    var relatives = JsonSerializer.Deserialize<List<Relative>>(relativesData);

                    if (relatives?.Count > 0)
                    {
                        await DbContext.Set<Relative>().AddRangeAsync(relatives);
                        await DbContext.SaveChangesAsync();
                    }
                }


                if (!await DbContext.Alerts.AnyAsync())
                {
                    var alertsData = await File.ReadAllTextAsync("../BFCAI.Nesyan.Infrastructure.Presistence/_Data/Seeds/alerts.json");
                    var alerts = JsonSerializer.Deserialize<List<Alert>>(alertsData);

                    if (alerts?.Count > 0)
                    {
                        await DbContext.Set<Alert>().AddRangeAsync(alerts);
                        await DbContext.SaveChangesAsync();
                    }
                }

                if (!await DbContext.MindGames.AnyAsync())
                {
                    var mindgamesData = await File.ReadAllTextAsync("../BFCAI.Nesyan.Infrastructure.Presistence/_Data/Seeds/mindgames.json");
                    var mindgames = JsonSerializer.Deserialize<List<MindGame>>(mindgamesData);

                    if (mindgames?.Count > 0)
                    {
                        await DbContext.Set<MindGame>().AddRangeAsync(mindgames);
                        await DbContext.SaveChangesAsync();
                    }
                }

                if (!await DbContext.Medications.AnyAsync())
                {
                    var medicationsData = await File.ReadAllTextAsync("../BFCAI.Nesyan.Infrastructure.Presistence/_Data/Seeds/medications.json");
                    var medications = JsonSerializer.Deserialize<List<Medication>>(medicationsData);

                    if (medications?.Count > 0)
                    {
                        await DbContext.Set<Medication>().AddRangeAsync(medications);
                        await DbContext.SaveChangesAsync();
                    }
                }

                if (!await DbContext.Reports.AnyAsync())
                {
                    var reportsData = await File.ReadAllTextAsync("../BFCAI.Nesyan.Infrastructure.Presistence/_Data/Seeds/reports.json");
                    var reports = JsonSerializer.Deserialize<List<Report>>(reportsData);

                    if (reports?.Count > 0)
                    {
                        await DbContext.Set<Report>().AddRangeAsync(reports);
                        await DbContext.SaveChangesAsync();
                    }
                }
                if (!await DbContext.PatientTelemetries.AnyAsync())
                {
                    var telemetryData = await File.ReadAllTextAsync("../BFCAI.Nesyan.Infrastructure.Presistence/_Data/Seeds/Telementry.json");
                    var telemetry = JsonSerializer.Deserialize<List<PatientTelemetry>>(telemetryData);

                    if (telemetry?.Count > 0)
                    {
                        await DbContext.Set<PatientTelemetry>().AddRangeAsync(telemetry);
                        await DbContext.SaveChangesAsync();
                    }
                }

                if (!await DbContext.Assessments.AnyAsync())
                {
                    var assessmentData = await File.ReadAllTextAsync("../BFCAI.Nesyan.Infrastructure.Presistence/_Data/Seeds/Assesment.json");
                    var assessments = JsonSerializer.Deserialize<List<Assessment>>(assessmentData);

                    if (assessments?.Count > 0)
                    {
                        await DbContext.Set<Assessment>().AddRangeAsync(assessments);
                        await DbContext.SaveChangesAsync();
                    }
                }
                if (!await DbContext.PatientRelatives.AnyAsync())
                {
                    var patientRelativesData = await File.ReadAllTextAsync("../BFCAI.Nesyan.Infrastructure.Presistence/_Data/Seeds/PatientRelative.json");
                    var patientRelatives = JsonSerializer.Deserialize<List<PatientRelative>>(patientRelativesData);

                    if (patientRelatives?.Count > 0)
                    {
                        await DbContext.Set<PatientRelative>().AddRangeAsync(patientRelatives);
                        await DbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during seeding: {ex.Message}");
            }
        }
    }
}
