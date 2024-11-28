namespace Chat.Models;

public class ServerFullInfo : ServerDetailInfo
{
    public List<Message> Messages { get; set;} = [];
}

public class ServerDetailInfo : ServerCoreInfo
{
    public int Capacity { get; set; } = 10;
    public List<User> ConnectedUsers { get; set; } = [];
}

public class ServerCoreInfo
{
    public Guid ServerId { get; set; }
    public string? ServerName { get; set; }
    public ServerType ServerType { get; set; }
    public bool Public { get; set; } = true;
}

public enum ServerType
{
    Chat = 0,
    Voice = 1
}