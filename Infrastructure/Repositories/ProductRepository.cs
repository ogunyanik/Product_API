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
    
    
    
    
    public Product GetById(int productId)
    {
        throw new NotImplementedException();
    }

    public Product Create(Product product)
    {
        throw new NotImplementedException();
    }

    public void Update(Product product)
    {
        throw new NotImplementedException();
    }

    public void Delete(int productId)
    {
        throw new NotImplementedException();
    }
}