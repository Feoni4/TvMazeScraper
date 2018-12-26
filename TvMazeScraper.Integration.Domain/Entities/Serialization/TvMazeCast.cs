using Newtonsoft.Json;

namespace TvMazeScraper.Integration.Domain.Entities
{
    public class TvMazeCast
    {
        [JsonProperty("person")]
        public TvMazePerson Person { get; set; }
    }
}