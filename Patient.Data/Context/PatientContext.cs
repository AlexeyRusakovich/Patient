using Microsoft.EntityFrameworkCore;
using Patient.Data.Models;

namespace Patient.Data.Context
{
    public class PatientContext : DbContext
    {
        public PatientContext(DbContextOptions<PatientContext> options) : base(options)
        {
        }

        public DbSet<Models.Patient> Patients { get; set; }
        public DbSet<PatientGivenName> GivenNames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientGivenName>(entity =>
            {
                entity.HasKey(e => new { e.PatientId, e.GivenName });

                entity.HasOne(d => d.Patient).WithMany(p => p.GivenNames).HasForeignKey(d => d.PatientId);
            });

            modelBuilder.Entity<Models.Patient>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasIndex(e => e.BirthDate);
            });
        }
    }
}
