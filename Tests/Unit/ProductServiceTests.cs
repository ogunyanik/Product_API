using Xunit;
using Moq;
using Product_API.Core.Models;
using Product_API.Core.Interfaces;
using Product_API.Core.Services;

namespace Product_API.Tests.Unit;

public class ProductServiceTests
{
    [Fact]
    public void GetProductById_ReturnsProduct_WhenProductExists()
    {
        // Arrange
        int productId = 1;
        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(productId))
            .Returns(new Task<Product>(() => new Product(){ProductId = 1, Title = "Product 1"}));
        var productService = new ProductService(mockRepository.Object);

        // Act
        var result = productService.GetProductByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
    }
}