using FluentValidation;

namespace CleanVidly.Controllers.Categories
{
    public class CategoryValidator : AbstractValidator<SaveCategoryResource>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Description).NotEmpty().MinimumLength(4).MaximumLength(64);
        }
    }
}