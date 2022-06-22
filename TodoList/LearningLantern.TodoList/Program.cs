using LearningLantern.Common;
using LearningLantern.Common.Logs;
using LearningLantern.TodoList;
using Serilog;

var builder = WebApplication.CreateBuilder(args).AddSerilog();

// Add services to the container.

builder.Services.AddApplication();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorizedSwaggerGen("TodoList.API", "v1");

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();