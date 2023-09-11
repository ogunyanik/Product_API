namespace Product_API.Core.DTO;

public class CategoryDTO
{ 
    public required string Id { get; set; }
    public int MinimumStockQuantity { get; set; } 
}