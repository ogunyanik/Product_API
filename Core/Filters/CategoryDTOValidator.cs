using FluentValidation;
using Product_API.Core.DTO;
using Product_API.Core.Models;
using Product_API.Core.Models.Enums;

namespace Product_API.Core.Filters;

public class CategoryDTOValidator: AbstractValidator<CategoryDTO>
{
    public CategoryDTOValidator()
    {
        RuleFor(category => category.Id)
            .NotEmpty()
            .IsEnumName(typeof(CategoryEnum), caseSensitive: false)
            .WithMessage("Category must be either 'Phone' or 'Tablet'");

        RuleFor(category => category.MinimumStockQuantity)
            .GreaterThan(0)
            .WithMessage("MinimumStockQuantity must be greater than 0");
    }
}