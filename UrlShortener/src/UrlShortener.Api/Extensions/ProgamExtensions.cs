using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;
using UrlShortener.Api.Services.Abstractions;
using UrlShortener.Api.Services.Core;

namespace UrlShortener.Api.Extensions
{
    public static class ProgamExtensions
    {
        public static void AddCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMemoryCache();

            var secretKey = Environment.GetEnvironmentVariable("UrlShortener_SecretKey", EnvironmentVariableTarget.User)
                ?? throw new InvalidOperationException("Environment variable not defined: 'UrlShortener_SecretKey'.");

            builder.Services.AddScoped<IUrlShortenerService>(_ =>
                new UrlShortenerService(_.GetRequiredService<IMemoryCache>(), secretKey));
        }

        public static async Task SeedUrls(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUrlShortenerService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                var urls = new[]
                {
                    "https://www.google.com",
                    "https://www.github.com",
                    "https://www.stackoverflow.com"
                };

                foreach (var url in urls)
                {
                    var result = await service.ShortenUrlAsync(url);
                    logger.LogInformation("Seed: {ShortUrl} -> {Url}", result, url);
                }
            }
        }
    }
}
