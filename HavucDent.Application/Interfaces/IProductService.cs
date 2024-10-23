using HavucDent.Domain.Entities;

namespace HavucDent.Application.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}