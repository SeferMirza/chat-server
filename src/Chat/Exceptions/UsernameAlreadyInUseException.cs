using System.Net;

namespace Chat.Exceptions;

public class UsernameAlreadyInUseException(string username)
    : HandledException(
        title: "Username Already In Use",
        httpStatusCode: HttpStatusCode.BadRequest,
        message: $"'{username}' already in use"
    );