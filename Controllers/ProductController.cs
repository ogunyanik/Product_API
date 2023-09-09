using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Product_API.Core.DTO;
using Product_API.Core.Filters;
using Product_API.Core.Interfaces;
using Product_API.Core.Models;
using AspNetCoreRateLimit;

namespace Product_API.Controllers;

[Route("api/products")]
[ApiController] 
public class ProductController : BaseController
{
    private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok(productDTO);
        }

        [HttpPost]
        [ValidateProductDTO]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            var createdProduct = await _productService.CreateProductAsync(product);
            var createdProductDTO = _mapper.Map<ProductDTO>(createdProduct);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProductDTO.ProductId }, createdProductDTO);
        }

        [HttpPut("{id}")]
        [ValidateProductDTO]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            var updatedProduct = await _productService.UpdateProductAsync(id, product);
            if (updatedProduct == null)
            {
                return NotFound();
            }

            var updatedProductDTO = _mapper.Map<ProductDTO>(updatedProduct);
            return Ok(updatedProductDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deletedProduct = await _productService.DeleteProductAsync(id);
            if (deletedProduct == null)
            {
                return NotFound();
            }

            var deletedProductDTO = _mapper.Map<ProductDTO>(deletedProduct);

            return Ok(deletedProductDTO);
        }

}