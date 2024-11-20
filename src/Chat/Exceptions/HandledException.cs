using System.Net;

namespace Chat.Exceptions;

public class HandledException(string title, HttpStatusCode httpStatusCode, string message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = httpStatusCode;
    public string Title { get; } = title;
}
