using Newtonsoft.Json;
using System;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Presentation.Entities
{
    public class Cast : ICast
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birthday")]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime? Birthday { get; set; }
    }
}