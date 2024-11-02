namespace Chat.Services;

[Serializable]
internal class ServerNotFound : Exception
{
    public ServerNotFound()
    {
    }

    public ServerNotFound(string? message) : base(message)
    {
    }

    public ServerNotFound(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}