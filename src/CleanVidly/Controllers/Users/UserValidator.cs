using FluentValidation;

namespace CleanVidly.Controllers.Users
{
    public class UserValidator : AbstractValidator<SaveUserResource>
    {
        public UserValidator()
        {
            RuleFor(u => u.Name).NotEmpty().MinimumLength(4).MaximumLength(32);
            RuleFor(u => u.Lastname).NotEmpty().MinimumLength(4).MaximumLength(32);
            RuleFor(u => u.Email).NotEmpty().MinimumLength(4).MaximumLength(128).EmailAddress();
            RuleFor(u => u.Roles).NotEmpty();
        }
    }
}