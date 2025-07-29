using Microsoft.EntityFrameworkCore;
using na4shtab.PatientApp.Models;

namespace na4shtab.PatientApp.Data
{
    public class PatientDbContext : DbContext
    {
        public DbSet<Patient> Patients     { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Visit> Visits         { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=patients.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visit>()
                .HasMany(v => v.SelectedProcedures)
                .WithMany(p => p.Visits)
                .UsingEntity(j => j.ToTable("VisitProcedures"));
        }
    }
}