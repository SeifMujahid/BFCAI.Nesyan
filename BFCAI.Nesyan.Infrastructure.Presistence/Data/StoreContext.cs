using BFCAI.Nesyan.Domain.Entities.Alerts;
using BFCAI.Nesyan.Domain.Entities.Medications;
using BFCAI.Nesyan.Domain.Entities.MindGames;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using BFCAI.Nesyan.Domain.Entities.Relations.Alerts;
using BFCAI.Nesyan.Domain.Entities.Relations.MindGames;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using BFCAI.Nesyan.Domain.Entities.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformation).Assembly);
        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Relative> Relatives { get; set; }
        public DbSet<Caregiver> Caregivers { get; set; }
        public DbSet<RelativeDoctorRequest> RelativeDoctorRequests { get; set; }
        public DbSet<PatientRelative>PatientRelatives { get; set; }
        public DbSet<Alert>Alerts { get; set; }
        public DbSet<PatientRelativeAlert>PatientRelativeAlerts { get; set; }

        public DbSet<MindGame>MindGames { get; set; }
        public DbSet<MindGameSession>MindGameSessions { get; set; }
        public DbSet<Report>Reports { get; set; }
        public DbSet<Medication> Medications { get; set; }


        //public DbSet<TreatmentRequest> TreatmentRequests { get; set; }
        //public DbSet<MindGame> MindGames { get; set; }
        //public DbSet<PatientMindGame> PatientMindGames { get; set; }
    }
}
