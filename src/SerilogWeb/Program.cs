using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SerilogWeb.Runtime;

namespace SerilogWeb
{
    public class Program
    {
        //public static int Main(string[] args)
        //{
        //    return AspNetCoreRuntime.Execute<Startup>(args);
        //}

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
