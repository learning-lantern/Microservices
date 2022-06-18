using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace LearningLantern.Common.Logs;

public static class SerilogMethods
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog(ConfigureLogger);
        return builder;
    }

    private static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
    {
        //set Minimum Level
        loggerConfiguration.MinimumLevel.Debug() //Default
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning);

        //set writeTo
        loggerConfiguration.WriteTo.Console()
            .WriteTo.Seq("http://seq");

        //set Enrich
        loggerConfiguration.Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId();

        if (context.HostingEnvironment.IsDevelopment())
        {
            loggerConfiguration.MinimumLevel.Override("LearningLantern", LogEventLevel.Debug);
        }
    }
}