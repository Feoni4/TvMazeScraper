using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TvMazeScraper.Presentation.Configurations;
using TvMazeScraper.Presentation.Extensions;
using TvMazeScraper.Presentation.Middleware;

namespace TvMazeScraper.Presentation
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
            services.Configure<ResponseCacheConfiguration>(Configuration.GetSection("ResponseCache"));
            services.Configure<DAL.StoreConfiguration>(Configuration.GetSection("StoreConfiguration"));

            services.AddLogging();
            services.AddApplicationInsightsTelemetry();

            services.AddAutoMapper();

            services.AddSingleton<DAL.Transport>((sp) => new DAL.Transport(sp.GetService<IOptions<DAL.StoreConfiguration>>().Value));
            services.AddTransient<Contracts.IKeyValueStore, DAL.KeyValueStore>();
            services.AddTransient<Contracts.IShowStore, DAL.ShowStore>();
            services.AddTransient<Domain.ISortedShowStore, Domain.SortedShowStore>();

            services.AddResponseCaching();
            services.AddResponseCompression();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCaching();
            app.UseResponseCacheConfigurationMiddleware();
            app.UseResponseCompression();

            app.UseMvc();
        }
    }
}
