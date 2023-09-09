using Product_API.Core.Interfaces;
using Product_API.Core.Models;
using Product_API.Infrastructure.Data;

namespace Product_API.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{

    private readonly AppDbContext _dbContext;
    public CategoryRepository(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    
    public Category GetById(int categoryId)
    {
        throw new NotImplementedException();
    }

    public Category Create(Category category)
    {
        throw new NotImplementedException();
    }

    public void Update(Category category)
    {
        throw new NotImplementedException();
    }

    public void Delete(int categoryId)
    {
        throw new NotImplementedException();
    }
}