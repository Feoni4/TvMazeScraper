using System.Collections.Generic;
using Newtonsoft.Json;

namespace TvMazeScraper.Integration.Domain.Entities
{
    public class TvMazeEmbededCast
    {
        [JsonProperty("cast")]
        public List<TvMazeCast> Casts { get; set; }
    }
}