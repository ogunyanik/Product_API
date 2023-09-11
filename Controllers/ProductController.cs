using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product_API.Core.DTO;
using Product_API.Core.Filters;
using Product_API.Core.Interfaces;
using Product_API.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace Product_API.Controllers;

[Route("api/v{version:apiVersion}/products")]
[ApiController] 
[Authorize(Policy = "StaticJwtTokenPolicy")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ProductController : BaseController
{
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IProductSearchService _productSearchService;

        public ProductController(
            IProductService productService, 
            IMapper mapper,
            IProductSearchService productSearchService)
        {
            _productService = productService;
            _mapper = mapper;
            _productSearchService = productSearchService;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products.Any())
            {
                return NotFound();
            }
            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productDTOs);
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "productId" })]
        [MapToApiVersion("1.0")]
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
        
        
        [HttpGet("{id}")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "productId" })]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetProductV2(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok(productDTO);
        }
        

        [HttpPost]
        [ValidateProductDTO]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            var createdProduct = await _productService.CreateProductAsync(product);
            var createdProductDTO = _mapper.Map<ProductDTO>(createdProduct);
            return CreatedAtAction(nameof(GetProduct), new { Title = createdProduct.Title   }, createdProductDTO);
        }

        [HttpPut("{id}")]
        [ValidateProductDTO]
        [MapToApiVersion("1.0")]
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
        [MapToApiVersion("1.0")]
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
        
        
        
        [HttpGet("search")]
        public async Task<IActionResult> SearchProductsAsync([FromQuery] string query)
        {
            var searchResponse = await _productSearchService.SearchAsync(query);

            if (!searchResponse.IsValid)
            {
                return StatusCode(500, "Error in Elasticsearch query");
            }

            var products = searchResponse.Documents;

            return Ok(products);
        }
        
        [HttpGet("filter")]
        public async Task<IActionResult> ProductFilterByQuantity([FromQuery] ProductFilterDTO filter)
        {
            
            var filteredProducts = await _productService.ProductFilterByQuantity(filter);

            if (filteredProducts.Any())
            {
                return NotFound();
            }
            return Ok(filteredProducts);
        }

}