using FluentValidation;

namespace CleanVidly.Controllers.Auth
{
    public class AuthValidator : AbstractValidator<AuthResource>
    {
        public AuthValidator()
        {
            RuleFor(a => a.Email).NotEmpty().MinimumLength(3).MaximumLength(128).EmailAddress();
            RuleFor(a => a.Password).NotEmpty().MinimumLength(3).MaximumLength(16);
        }
    }
}