using Newtonsoft.Json.Converters;

namespace TvMazeScraper.Presentation
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}