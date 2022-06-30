using LearningLantern.ApiGateway.Data;
using LearningLantern.ApiGateway.Tests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LearningLantern.ApiGateway.Tests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ILearningLanternContext));
            services.Remove(descriptor);

            services.AddDbContext<ILearningLanternContext, LearningLanternContextMock>(
                builder => { builder.UseInMemoryDatabase("LearningLantern"); }
            );

            using var scope = services.BuildServiceProvider().CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<LearningLanternContextMock>();
            db.Database.EnsureCreated();
        });
    }
}