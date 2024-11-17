namespace Chat.Models;

public record ServerDetail(Guid ServerId, string ServerName, int Capacity, List<string> ConnectedUsers);
