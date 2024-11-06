﻿using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Repositories;

namespace HavucDent.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Unit of work'ü sadece transaction yönetimi için kullanmak ve
        //her servisin tüm unit of worke bağımlı olmamasını ve
        //her servisin ihtiyacı olan repository kendi servis katmanına eklemek ve
        //modüler hale getirmek için refactör işlemi yapıldı

        IRepository<Product> Products { get; }
        IRepository<Appointment> Appointments { get; }
        IRepository<Doctor> Doctors { get; }
        IRepository<Assistant> Assistants { get; }
        IRepository<Patient> Patients { get; }
        IRepository<Laboratory> Laboratories { get; }
        IRepository<User> Users { get; }


		Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task SaveChangesAsync();
    }
}