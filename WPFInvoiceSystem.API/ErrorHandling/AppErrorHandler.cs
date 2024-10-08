using Microsoft.AspNetCore.Diagnostics;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WPFInvoiceSystem.Domain.Exceptions;

namespace WPFInvoiceSystem.API.ErrorHandling
{
    public class AppErrorHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken
            )
        {
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            IResult result = MapException(exception, traceId);
            await result.ExecuteAsync(httpContext);

            return true;
        }

        private static IResult MapException(Exception exception, string traceId)
        {
            string problemTitle = "An error occurred while processing your request.";
            int problemStatusCode = StatusCodes.Status500InternalServerError;
            var errorMessages = new Collection<string>();
            var problemExtensions = new Dictionary<string, object?>();

            if (exception is CoreException)
            {
                if (exception is CoreValidationException)
                {
                    problemTitle = "One or more validation errors ocurred";
                    problemStatusCode = StatusCodes.Status400BadRequest;
                }

                if (exception is CoreNotFoundException)
                {
                    problemTitle = "Not found";
                    problemStatusCode = StatusCodes.Status404NotFound;
                }

                if (exception is CoreActionForbiddenException)
                {
                    problemTitle = "Acción no permitida";
                    problemStatusCode = StatusCodes.Status403Forbidden;
                }

                errorMessages.Add(exception.Message);
            }

            problemExtensions.Add("traceId", traceId);

            if (errorMessages.Any()) 
            {
                problemExtensions.Add("errors", errorMessages);
            }

            return Results.Problem(
                title: problemTitle,
                statusCode: problemStatusCode,
                extensions: problemExtensions
                );
        }
    }
}
