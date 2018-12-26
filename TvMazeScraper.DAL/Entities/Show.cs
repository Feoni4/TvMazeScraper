using Newtonsoft.Json;
using System.Collections.Generic;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.DAL.Entities
{
    public class Show : IShow
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(InterfaceConverter<ICast, Cast>))]
        public List<ICast> Cast { get; }

        public Show()
        {
            Cast = new List<ICast>();
        }
    }
}