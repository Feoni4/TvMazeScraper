using System.Collections.Generic;
using TvMazeScraper.Contracts.Entities;
using Newtonsoft.Json;

namespace TvMazeScraper.Presentation.Entities
{
    public class Show : IShow
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cast")]
        public List<ICast> Cast { get; set; }
    }
}