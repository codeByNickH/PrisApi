using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PrisApi.ExceptionHandler
{
    public class ExceptionHandler : IExceptionFilter
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ExceptionHandler(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var isDevelopment = _webHostEnvironment.IsDevelopment();

            // Build error details conditionally based on environment
            string errorDetails = null;
            if (isDevelopment)
            {
                errorDetails = $"{exception.Message}\n\nStackTrace:\n{exception.StackTrace}";
            }

            context.Result = new ObjectResult(new
            {
                message = "An error occurred",
                details = errorDetails,
            })
            {
                StatusCode = 500,
            };
            context.ExceptionHandled = true;
        }
    }
}