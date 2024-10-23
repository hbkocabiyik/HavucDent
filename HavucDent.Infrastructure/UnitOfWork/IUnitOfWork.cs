using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Repositories;

namespace HavucDent.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<Appointment> Appointments { get; }
        IRepository<Doctor> Doctors { get; }
        IRepository<Assistant> Assistants { get; }
        IRepository<Patient> Patients { get; }
        IRepository<Laboratory> Laboratories { get; }

        Task SaveChangesAsync();
    }
}
