using FluentValidation;
using Product_API.Core.DTO;

namespace Product_API.Core.Filters;

public class ProductDTOValidator: AbstractValidator<ProductDTO>
{
    public ProductDTOValidator()
    {
        RuleFor(product => product.Title).NotEmpty().MaximumLength(200);
        RuleFor(product => product.Description).NotEmpty();
        RuleFor(product => product.StockQuantity).GreaterThanOrEqualTo(0); 
        RuleFor(product => product.CategoryDto)
            .NotNull()
            .SetValidator(new CategoryDTOValidator());
    }
}

