using Microsoft.EntityFrameworkCore;
using Product_API.Core.DTO;
using Product_API.Core.Interfaces;
using Product_API.Core.Models;
using Product_API.Infrastructure.Data;

namespace Product_API.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext; 

    public ProductRepository(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }


    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int productId)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _dbContext.Entry(product).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> DeleteAsync(Product product)
    {
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<IEnumerable<Product>> FilterProductsAsync(ProductFilterDTO filter)
    {
        return await _dbContext.Products
            .Where(p => p.StockQuantity >= filter.MinStockQuantity && p.StockQuantity <= filter.MaxStockQuantity)
            .ToListAsync();
    }
}