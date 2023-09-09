using Product_API.Core.Models;

namespace Product_API.Core.Interfaces;

public interface IProductRepository
{ 
    Task<List<Product>> GetAllProducts();

    Task<Product?> GetById(long id);
}