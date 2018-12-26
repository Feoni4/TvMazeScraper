using AutoMapper;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.Presentation.Entities;

namespace TvMazeScraper.Integration.Domain
{
    public class PresentationAutoMapperProfile : Profile
    {
        public PresentationAutoMapperProfile()
        {
            CreateMap<DAL.Entities.Show, Show>();
            CreateMap<DAL.Entities.Cast, Cast>();
            CreateMap<IShow, Show>();
            CreateMap<ICast, Cast>();
        }
    }
}