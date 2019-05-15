using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SerilogWeb.Runtime
{
    public static class AspNetCoreRuntime
    {
        public static int Execute<TStartClass>(string[] args) where TStartClass : class
        {
            try
            {
                IHostBuilder builder = Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseAppHostOptions();
                });


                IHost host = builder.Build();
                // here you can start with logging
                Log.Information("Host pipeline setup.");

                Log.Information("Starting Webhost");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return -1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

            return 0;
        }


        private static IWebHostBuilder UseAppHostOptions(this IWebHostBuilder webHost)
        {
            return webHost
                // Configuration
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    IWebHostEnvironment env = builderContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: false,
                            reloadOnChange: true) // default file
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true,
                            reloadOnChange: true) // environment file for local development
                        .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true,
                            reloadOnChange: true) // environment file for local machine
                        .AddEnvironmentVariables(); // Docker environment support
                })
                // Logging
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
                })
                // Register Error Handling
                .UseSetting("detailedErrors", "true")
                .CaptureStartupErrors(true);
        }
    }
}
