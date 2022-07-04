using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace LearningLantern.Common.Logging;

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
        loggerConfiguration.MinimumLevel.Information() //Default
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("LearningLantern", LogEventLevel.Debug);

        //set writeTo
        loggerConfiguration.WriteTo.Console()
            .WriteTo.Seq("https://learning-lantern-logs.azurewebsites.net");

        //set Enrich
        loggerConfiguration.Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId();
    }
}