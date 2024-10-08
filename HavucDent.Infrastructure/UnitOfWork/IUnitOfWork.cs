using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Repositories;

namespace HavucDent.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        Task SaveChangesAsync();
    }
}
