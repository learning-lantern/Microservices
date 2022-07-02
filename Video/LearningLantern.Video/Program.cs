using LearningLantern.AzureBlobStorage;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Logging;
using LearningLantern.Video;
using Serilog;

var builder = WebApplication.CreateBuilder(args).AddSerilog();

// Add services to the container.

builder.Services.AddBlobService(builder.Configuration["BlobStorageConnectionString"], "videos");
builder.Services.AddApplication();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorizedSwaggerGen("Video.API", "v1");


// Configure the HTTP request pipeline.
var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();