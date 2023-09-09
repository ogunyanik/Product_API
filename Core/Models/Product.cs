namespace Product_API.Core.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public int StockQuantity { get; set; } 
}