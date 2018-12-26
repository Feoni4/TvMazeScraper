using AutoMapper;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.Integration.Domain.Entities;

namespace TvMazeScraper.Integration.Domain
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TvMazeShow, Show>()
                .ForMember(m => m.Cast, cfg => cfg.MapFrom(f => f.Embeded.Casts));

            CreateMap<TvMazeCast, ICast>()
                .ConstructUsing(cast => new Cast { Id = cast.Person.Id, Name = cast.Person.Name, Birthday = cast.Person.Birthday });
        }
    }
}