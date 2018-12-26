using Newtonsoft.Json;

namespace TvMazeScraper.Integration.Domain.Entities
{
    public class TvMazeShow
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("_embedded")]
        public TvMazeEmbededCast Embeded { get; set; }
    }
}