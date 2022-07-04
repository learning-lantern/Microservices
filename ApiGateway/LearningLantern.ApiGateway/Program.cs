using LearningLantern.ApiGateway.Configurations;
using LearningLantern.ApiGateway.Users.Events;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Logging;
using LearningLantern.EventBus;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args).AddSerilog();

ConfigProvider.Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationConfiguration();
builder.AddOcelotConfiguration();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorizedSwaggerGen("ApiGateway.API", "v1");


var app = builder.Build();
app.UseSerilogRequestLogging();

app.UseCors();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
    endpoints.MapControllers()
);


app.UseSwagger();
app.UseSwaggerForOcelotUI(options => { options.PathToSwaggerGenerator = "/swagger/docs"; }).UseOcelot().Wait();
//app.UseSwaggerUI();


using (var scope = app.Services.CreateScope())
{
    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
    Task.Run(() =>
    {
        Task.Delay(1000);
        if (!eventBus.SetupConfiguration()) return;
        Log.Logger.Debug("SetupConfiguration done");
        eventBus.AddEvent<UserEvent>("auth");
        eventBus.AddEvent<DeleteUserEvent>("auth");
        eventBus.Subscribe("chat");
        Log.Logger.Debug("Subscribe done");
    });
}

app.Run();

namespace LearningLantern.ApiGateway
{
    public class Program
    {
    }
}