using System.Threading.Tasks;
using CleanVidly.Controllers.Resources;
using Microsoft.AspNetCore.Mvc;

namespace CleanVidly.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static BadRequestObjectResult BadRequest(this ControllerBase controllerBase, string key, string error)
        {
            var errorResource = new ValidationErrorResource()
            {
                Status = 400,
                Title = "One or more validation errors occurred.",
                TraceId = "Custom validation"
            };

            errorResource.Errors.Add(key, new string[] { error });
            return controllerBase.BadRequest(errorResource);
        }
    }
}