using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Persistence;
using HavucDent.Infrastructure.Repositories;

namespace HavucDent.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HavucDbContext _context;
        private IRepository<Product> _products;

        public UnitOfWork(HavucDbContext context)
        {
            _context = context;
        }

        public IRepository<Product> Products => _products ??= new Repository<Product>(_context);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
