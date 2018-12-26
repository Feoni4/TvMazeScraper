using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace TvMazeScraper.Integration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Domain.AutoMapperProfile>();
            });

            services.AddSingleton<IMapper>(sp => config.CreateMapper());
        }
    }
}