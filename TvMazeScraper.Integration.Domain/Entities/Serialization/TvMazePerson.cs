using Newtonsoft.Json;
using System;

namespace TvMazeScraper.Integration.Domain.Entities
{
    public class TvMazePerson
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birthday")]
        [JsonConverter(typeof(FailSafeDateConverter))]
        public DateTime? Birthday { get; set; }
    }
}