﻿using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Persistence;
using HavucDent.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using HavucDent.Infrastructure.Interfaces;

namespace HavucDent.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HavucDbContext _context;
        private IDbContextTransaction _transaction;

        private IRepository<Product> _products;
        private IRepository<Appointment> _appointments;
        private IRepository<Doctor> _doctors;
        private IRepository<Assistant> _assistants;
        private IRepository<Patient> _patients;
        private IRepository<Laboratory> _laboratories;
        private IRepository<User> _users;

		public UnitOfWork(HavucDbContext context)
        {
            _context = context;
        }

        public IRepository<Product> Products => _products ??= new Repository<Product>(_context);
        public IRepository<Appointment> Appointments => _appointments ??= new Repository<Appointment>(_context);
        public IRepository<Doctor> Doctors => _doctors ??= new Repository<Doctor>(_context);
        public IRepository<Assistant> Assistants => _assistants ??= new Repository<Assistant>(_context);
        public IRepository<Patient> Patients => _patients ??= new Repository<Patient>(_context);
        public IRepository<Laboratory> Laboratories => _laboratories ??= new Repository<Laboratory>(_context);
        public IRepository<User> Users => _users ??= new Repository<User>(_context);

		public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}