using HavucDent.Domain.Entities;

namespace HavucDent.Application.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
