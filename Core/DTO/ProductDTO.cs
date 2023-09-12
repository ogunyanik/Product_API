using Product_API.Core.Models;

namespace Product_API.Core.DTO;

public class ProductDTO
{ 
    public string Title { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    
    public int CategoryId{ get; set; }
}