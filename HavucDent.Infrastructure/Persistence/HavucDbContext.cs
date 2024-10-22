using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HavucDent.Infrastructure.Persistence
{
    public class HavucDbContext : IdentityDbContext<AppUser>
    {
        public HavucDbContext(DbContextOptions<HavucDbContext> options) : base(options)
        {
        }

        // DbSet tanımlamaları
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Admin> Admins { get; set; }  
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<LeaveHistory> LeaveHistories { get; set; }
        public DbSet<SalaryHistory> SalaryHistories { get; set; }
        public DbSet<AccessControl> AccessControls { get; set; }
        public DbSet<Laboratory> Laboratories { get; set; } 
        public DbSet<AppointmentLaboratory> AppointmentLaboratories { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API kullanarak entity'ler arasındaki ilişkiler ve konfigürasyonlar

            // User tabanı sınıfının konfigürasyonu
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Doctor>("Doctor")
                .HasValue<Assistant>("Assistant")
                .HasValue<Admin>("Admin");

            // Appointment - Doctor ilişkisi (1:Many)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId);

            // Appointment - Assistant ilişkisi (1:Many)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Assistant)
                .WithMany(a => a.AssignedAppointments)
                .HasForeignKey(a => a.AssistantId);

            // Appointment - Patient ilişkisi (1:Many)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId);

            // Appointment - Product ilişkisi (Many:Many)
            modelBuilder.Entity<Appointment>()
                .HasMany(a => a.UsedProducts)
                .WithMany(p => p.Appointments)
                .UsingEntity(j => j.ToTable("AppointmentProducts"));

            // LeaveHistory - User ilişkisi (1:Many)
            modelBuilder.Entity<LeaveHistory>()
                .HasOne(l => l.User)
                .WithMany(u => u.LeaveHistories)
                .HasForeignKey(l => l.UserId);

            // SalaryHistory - User ilişkisi (1:Many)
            modelBuilder.Entity<SalaryHistory>()
                .HasOne(s => s.User)
                .WithMany(u => u.SalaryHistories)
                .HasForeignKey(s => s.UserId);

            // Appointment - AppointmentLaboratory ilişkisi (Many:Many)
            modelBuilder.Entity<AppointmentLaboratory>()
                .HasOne(al => al.Appointment)
                .WithMany(a => a.AppointmentLaboratories)
                .HasForeignKey(al => al.AppointmentId);

            modelBuilder.Entity<AppointmentLaboratory>()
                .HasOne(al => al.Laboratory)
                .WithMany(l => l.AppointmentLaboratories)
                .HasForeignKey(al => al.LaboratoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
