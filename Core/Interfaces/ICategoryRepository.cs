using Product_API.Core.Models;

namespace Product_API.Core.Interfaces;

public interface ICategoryRepository
{
    Category GetById(int categoryId);
    Category Create(Category category);
    void Update(Category category);
    void Delete(int categoryId);
}