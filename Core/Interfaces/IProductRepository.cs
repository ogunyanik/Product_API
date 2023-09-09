using Product_API.Core.Models;

namespace Product_API.Core.Interfaces;

public interface IProductRepository
{ 
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int productId);
    Task<Product> AddAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<Product> DeleteAsync(Product product);
}