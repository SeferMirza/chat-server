using Chat.Hubs;

namespace Chat;

public static class WebApplicationExtensions
{
    public static void UseHubs(this WebApplication webApplication)
    {
        webApplication.MapHub<ChatChannelHub>("/chatHub");
        webApplication.MapHub<VoiceChannelHub>("/voiceHub");
    }
}
