using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product_API.Core.DTO;
using Product_API.Core.Interfaces;
using Product_API.Core.Models;

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
        public IActionResult GetProducts()
        {
            // Retrieve a list of products from the ProductService
            var products = _productService.GetAllProducts();

            // Map the domain models to DTOs
            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productDTOs);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            // Retrieve a single product by ID from the ProductService
            // var product = _productService.GetProductById(id);
            var product = new Product();
            if (product == null)
            {
                return NotFound();
            }

            // Map the domain model to a DTO
            var productDTO = _mapper.Map<ProductDTO>(product);

            return Ok(productDTO);
        }

        // [HttpPost]
        // public IActionResult CreateProduct([FromBody] ProductDTO productDTO)
        // {
        //     // Map the incoming DTO to a domain model
        //     var product = _mapper.Map<Product>(productDTO);
        //
        //     // Use the ProductService to create the product
        //     var createdProduct = _productService.CreateProduct(product);
        //
        //     // Map the created product back to a DTO
        //     var createdProductDTO = _mapper.Map<ProductDTO>(createdProduct);
        //
        //     return CreatedAtAction(nameof(GetProduct), new { id = createdProductDTO.ProductId }, createdProductDTO);
        // }
        //
        // [HttpPut("{id}")]
        // public IActionResult UpdateProduct(int id, [FromBody] ProductDTO productDTO)
        // {
        //     // Map the incoming DTO to a domain model
        //     var product = _mapper.Map<Product>(productDTO);
        //
        //     // Use the ProductService to update the product
        //     var updatedProduct = _productService.UpdateProduct(id, product);
        //
        //     if (updatedProduct == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     // Map the updated product back to a DTO
        //     var updatedProductDTO = _mapper.Map<ProductDTO>(updatedProduct);
        //
        //     return Ok(updatedProductDTO);
        // }
        //
        // [HttpDelete("{id}")]
        // public IActionResult DeleteProduct(int id)
        // {
        //     // Use the ProductService to delete the product
        //     var deletedProduct = _productService.DeleteProduct(id);
        //
        //     if (deletedProduct == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     // Map the deleted product back to a DTO
        //     var deletedProductDTO = _mapper.Map<ProductDTO>(deletedProduct);
        //
        //     return Ok(deletedProductDTO);
        // }
    
}