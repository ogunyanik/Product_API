using Product_API.Core.Models;

namespace Product_API.Core.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> GetProductByIdAsync(int productId);
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(int productId, Product product);
    Task<Product> DeleteProductAsync(int productId);
}