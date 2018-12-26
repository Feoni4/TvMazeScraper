using System.Net.Http;

namespace TvMazeScraper.Integration.Domain.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static bool IsRateLimitExceed(this HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 429:
                case 500:
                    return true;
                default:
                    return false;
            }
        }
    }
}