using FluentValidation;
using Product_API.Core.DTO;
using Product_API.Core.Models;
using Product_API.Core.Models.Enums;

namespace Product_API.Core.Filters;

public class CategoryDTOValidator: AbstractValidator<int>
{
    public CategoryDTOValidator()
    {
        RuleFor(x => x)
            .Must(BeAValidCategoryEnumValue)
            .WithMessage("Category must be a valid CategoryId -- Check documentation for valid values (there is no documentation yet :) ).");
 
    }
    
    private static bool BeAValidCategoryEnumValue(int categoryId)
    {
        return Enum.IsDefined(typeof(CategoryEnum), categoryId);
    }
}