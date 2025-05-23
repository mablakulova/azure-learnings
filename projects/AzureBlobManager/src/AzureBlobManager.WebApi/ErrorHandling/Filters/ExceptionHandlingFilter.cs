using AzureBlobManager.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AzureBlobManager.WebApi.ErrorHandling.Filters;

public class ExceptionHandlingFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = GetExceptionResult(context.Exception);
        context.ExceptionHandled = true;
    }

    private static IActionResult GetExceptionResult(Exception exception)
        => exception switch
        {
            InputValidationException e => HandleValidationException(e),
            EntityNotFoundException e => HandleNotFoundException(e),
            _ => HandleUnknownException(exception)
        };

    private static IActionResult HandleUnknownException(Exception _)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        return new ObjectResult(details) { StatusCode = StatusCodes.Status500InternalServerError };
    }

    private static IActionResult HandleValidationException(InputValidationException exception)
    {
        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        return new BadRequestObjectResult(details);
    }

    private static IActionResult HandleNotFoundException(EntityNotFoundException exception)
    {
        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        return new NotFoundObjectResult(details);
    }
}