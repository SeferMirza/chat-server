using Chat;
using Chat.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSignalR();
builder.Services.AddOurServices();

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
app.UseHubs();

app.Run();
