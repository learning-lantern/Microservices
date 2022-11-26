using LearningLantern.Admin.Repositories;
using LearningLantern.ApiGateway.Configurations;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args).AddSerilog();
builder.Configuration.AddJsonFile("appsettings.json");
ConfigProvider.Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddTransient<IAdminRepository, AdminRepository>();
builder.Services.AddControllers();
builder.Services.AddApplicationConfiguration();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorizedSwaggerGen("Admin.API", "v1");


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
app.UseSwaggerUI();


app.Run();