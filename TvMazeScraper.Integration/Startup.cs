using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using TvMazeScraper.Integration.Domain;
using TvMazeScraper.Integration.Extensions;
using TvMazeScraper.Integration.Jobs;

namespace TvMazeScraper.Integration
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddApplicationInsightsTelemetry();

            services.AddAutoMapper();

            services.Configure<DAL.StoreConfiguration>(Configuration.GetSection("StoreConfiguration"));

            services.AddTransient<Domain.TvMazeApiClient>();
            services.AddTransient<ShowSynchronization>();
            services.AddSingleton<DAL.Transport>((sp) => new DAL.Transport(sp.GetService<IOptions<DAL.StoreConfiguration>>().Value));
            services.AddTransient<Contracts.IKeyValueStore, DAL.KeyValueStore>();
            services.AddTransient<Contracts.IShowStore, DAL.ShowStore>();

            services.AddHostedService<ShowSynchronizationHostedService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddApplicationInsights(serviceProvider);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}