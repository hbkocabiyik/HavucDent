using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Persistence;

namespace HavucDent.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IRepository<Product>
    {
        public ProductRepository(HavucDbContext context) : base(context)
        {
        }

        // Product ile ilgili özel metodlar ekle
    }
}