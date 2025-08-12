using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PrisApi.ExcetionHandler
{
    public class ExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            context.Result = new ObjectResult(new
            {
                message = "An Error has occurred",
                details = exception.Message,
            })
            {
                StatusCode = 500,
            };
            context.ExceptionHandled = true;
                                                                // Add this in Program.cs later.
        }
    }
}