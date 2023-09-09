using Product_API.Core.Interfaces;
using Product_API.Core.Models;

namespace Product_API.Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }


    public Task<List<Product>> GetAllProducts()
    {
        throw new NotImplementedException();
    }
}