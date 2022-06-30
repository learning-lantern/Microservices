using Azure.Storage.Blobs;
using LearningLantern.Video;
using LearningLantern.Video.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplication();
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration["BlobStorageConnectionString"]));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure the HTTP request pipeline.
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
