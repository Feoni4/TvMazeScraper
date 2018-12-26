using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace TvMazeScraper.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DAL.Entities.Show, Entities.Show>();
                cfg.CreateMap<DAL.Entities.Cast, Entities.Cast>();
                cfg.CreateMap<Contracts.Entities.IShow, Entities.Show>();
                cfg.CreateMap<Contracts.Entities.ICast, Entities.Cast>();
            });

            services.AddSingleton<IMapper>(sp => config.CreateMapper());
        }
    }
}