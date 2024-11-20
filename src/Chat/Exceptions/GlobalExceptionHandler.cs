using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace Chat.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";
        APIResponse<object> response;
        if(exception is HandledException handledException)
        {
            response = APIResponse<object>.ErrorResponse(error: handledException.Title, message: handledException.Message, statusCode: handledException.StatusCode);
        }
        else
        {
            response = APIResponse<object>.ErrorResponse(error: "Internal Server Error", message: "Internal Server Error", statusCode: HttpStatusCode.InternalServerError);
        }

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
