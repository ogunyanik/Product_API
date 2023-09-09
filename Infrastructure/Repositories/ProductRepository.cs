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


    public async Task<List<Product>> GetAllProducts()
    {
        throw new NotImplementedException();
    }

    public async Task<Product?> GetById(long id)
    {
        throw new NotImplementedException();
    }
}