using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace TvMazeScraper.Integration.Domain
{
    public class FailSafeDateConverter : DateTimeConverterBase
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dateString = reader.Value as string;
            if (string.IsNullOrEmpty(dateString))
                return default(DateTime?);

            if (!DateTime.TryParse(dateString, out var date))
                return default(DateTime?);

            return date;
        }
    }
}