using System.Net;

namespace Chat.Exceptions;

public class UserDidNotJoinThisServerException()
    : HandledException(
        title: "User Not Join Server",
        httpStatusCode: HttpStatusCode.Unauthorized,
        message: $"User did not join this server!"
    );