using Chat.Services;

namespace Chat;

public static class ServicesExtensions
{
    public static void AddOurServices(this IServiceCollection  services)
    {
        services.AddKeyedSingleton<IService, ChatChannelService>(nameof(ChatChannelService));
        services.AddKeyedSingleton<IService, VoiceChannelService>(nameof(VoiceChannelService));
    }
}
