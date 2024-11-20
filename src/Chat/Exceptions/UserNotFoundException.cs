using System.Net;

namespace Chat.Exceptions;

public class UserNotFoundException()
    : HandledException(
        title: "User Not Found",
        httpStatusCode: HttpStatusCode.NotFound,
        message: $"User not found!"
    );