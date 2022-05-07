using LearningLantern.Common.DependencyInjection;
using LearningLantern.Common.Logging;
using LearningLantern.TodoList;
using Serilog;

var builder = WebApplication.CreateBuilder(args).AddSerilog();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorizedSwaggerGen("Calendar.API", "v1");

builder.Services.AddApplication();


var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();