namespace Product_API.Core.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public int MinimumStockQuantity { get; set; } 
}