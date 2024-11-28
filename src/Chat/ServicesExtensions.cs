using Chat.Services;

namespace Chat;

public static class ServicesExtensions
{
    public static void AddOurServices(this IServiceCollection  services)
    {
        services.AddSingleton<IService, ServerService>();
    }
}
