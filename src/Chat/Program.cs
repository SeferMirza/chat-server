using Chat.Exceptions;
using Chat.Hubs;
using Chat.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSignalR();
builder.Services.AddSingleton<IChatService, ChatService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ChatPolicy", builder =>
    {
        builder.SetIsOriginAllowed(_ => true)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("ChatPolicy");
app.UseRouting();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
