using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Product_API.Core.DTO;

namespace Product_API.Core.Filters;

public class ValidateProductDTOAttribute: TypeFilterAttribute
{
    public ValidateProductDTOAttribute() : base(typeof(ProductDTOValidatorActionFilter))
    {
    }

    private class ProductDTOValidatorActionFilter : IActionFilter
    {
        private readonly IValidator<ProductDTO> _validator;

        public ProductDTOValidatorActionFilter(IValidator<ProductDTO> validator)
        {
            _validator = validator;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("productDTO", out var value) && value is ProductDTO productDTO)
            {
                // Validate the incoming ProductDTO using FluentValidation
                var validationResult = _validator.Validate(productDTO);

                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.Errors);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Optionally, you can perform actions after the action method is executed.
        }
    }
}