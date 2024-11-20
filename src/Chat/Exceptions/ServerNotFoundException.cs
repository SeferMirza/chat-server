using System.Net;

namespace Chat.Exceptions;

public class ServerNotFoundException(Guid serverId)
    : HandledException(
        title: "Server Not Found",
        httpStatusCode: HttpStatusCode.NotFound,
        message: $"Server '{serverId}' not found!"
    );