using LearningLantern.ApiGateway.Configurations;
using LearningLantern.Common.DependencyInjection;
using LearningLantern.Common.Logging;
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
app.UseSwaggerUI();

app.AddRabbitMQConfiguration();

app.Run();

namespace LearningLantern.ApiGateway
{
    public partial class Program { }
}
