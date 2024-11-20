using System.Net;

namespace Chat;

public class APIResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    public string? Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public static APIResponse<T> SuccessResponse(T data, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new APIResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            StatusCode = statusCode
        };
    }

    public static APIResponse<T> ErrorResponse(string error, string? message = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        return new APIResponse<T>
        {
            Success = false,
            Error = error,
            Message = message,
            StatusCode = statusCode
        };
    }
}
