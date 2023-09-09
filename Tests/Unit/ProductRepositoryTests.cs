using Xunit;
using Moq;
using Product_API.Infrastructure.Data;
using Product_API.Infrastructure.Repositories;
using Product_API.Core.Models;
 

public class ProductRepositoryTests
{
    [Fact]
    public void GetProductById_ReturnsProduct_WhenProductExists()
    {
        // Arrange
        int productId = 1;
        var mockDbContext = new Mock<AppDbContext>(); // Replace with your actual DbContext
        mockDbContext.Setup(db => db.Products.Find(productId))
            .Returns(new Product { ProductId = productId, Title = "Product 1" });
        var productRepository = new ProductRepository(mockDbContext.Object);

        // Act
        var result = productRepository.GetByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
    }

    // Add more unit tests for other ProductRepository methods
}