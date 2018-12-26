using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TvMazeScraper.Presentation.Configurations;

namespace TvMazeScraper.Presentation.Middleware
{
    public class ResponseCacheConfigurationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ResponseCacheConfiguration _config;

        public ResponseCacheConfigurationMiddleware(RequestDelegate next, ResponseCacheConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public Task Invoke(HttpContext httpContext)
        {
            SetResponseCache(httpContext);

            return _next(httpContext);
        }

        private void SetResponseCache(HttpContext context)
        {
            context.Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(_config.MaxAgeInSeconds)
            };
            context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new string[] { "Accept-Encoding" };
        }
    }

    public static class ResponseCacheConfigurationMiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseCacheConfigurationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseCacheConfigurationMiddleware>();
        }
    }
}