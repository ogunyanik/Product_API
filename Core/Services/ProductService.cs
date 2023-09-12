using Product_API.Core.DTO;
using Product_API.Core.Interfaces;
using Product_API.Core.Models;
using Product_API.Core.Models.Enums;

namespace Product_API.Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> GetProductByIdAsync(int productId)
    {
        return await _productRepository.GetByIdAsync(productId);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        // Add business logic or validation as needed
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        } 
        return await _productRepository.AddAsync(product);
    }

    public async Task<Product> UpdateProductAsync(int productId, Product product)
    {
        // Add business logic or validation as needed
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var existingProduct = await _productRepository.GetByIdAsync(productId);

        if (existingProduct == null)
        {
            return null; // Product not found
        }

        existingProduct.Title = product.Title;
        existingProduct.Description = product.Description;
        existingProduct.StockQuantity = product.StockQuantity; 
        
        
        return await _productRepository.UpdateAsync(existingProduct);
    }

    public async Task<Product> DeleteProductAsync(int productId)
    {
        var existingProduct = await _productRepository.GetByIdAsync(productId);

        if (existingProduct == null)
        {
            return null; // Product not found
        }

        return await _productRepository.DeleteAsync(existingProduct);
    }

    public async Task<IEnumerable<Product>> ProductFilterByQuantity(ProductFilterDTO productFilterDto)
    {
        return await _productRepository.FilterProductsAsync(productFilterDto);
    }
}