namespace Product_API.Core.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    public int CategoryId { get; set; } 
    public int StockQuantity { get; set; } 
    
    public Category Category { get; set; }
}