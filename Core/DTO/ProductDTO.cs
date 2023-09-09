namespace Product_API.Core.DTO;

public class ProductDTO
{
    public int ProductId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
}