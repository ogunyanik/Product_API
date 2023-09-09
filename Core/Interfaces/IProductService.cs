using Product_API.Core.Models;

namespace Product_API.Core.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllProducts(); 
}