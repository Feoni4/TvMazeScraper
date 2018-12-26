using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TvMazeScraper.Integration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile($"appsettings.store.json", optional: true, reloadOnChange: true);
                })
                .UseStartup<Startup>();
    }
}