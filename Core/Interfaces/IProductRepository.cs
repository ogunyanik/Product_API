using Product_API.Core.Models;

namespace Product_API.Core.Interfaces;

public interface IProductRepository
{
    Product GetById(int productId);
    Product Create(Product product);
    void Update(Product product);
    void Delete(int productId);
}