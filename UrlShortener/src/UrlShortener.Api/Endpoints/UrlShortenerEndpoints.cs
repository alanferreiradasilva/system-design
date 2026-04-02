using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Endpoints.Models;
using UrlShortener.Api.Extensions;
using UrlShortener.Api.Services.Abstractions;

namespace UrlShortener.Api.Endpoints
{
    public class UrlShortenerEndpoints : IEndpointMapper
    {
        public void Map(WebApplication app)
        {
            app.MapGet("/url-shortener/all", async ([FromServices] IUrlShortenerService service) =>
            {
                return await service.GetAllUrlsAsync();
            })
            .WithName("GetAllUrlShortener")
            .WithOpenApi();

            app.MapGet("/url-shortener", async (string shortUrl, [FromServices] IUrlShortenerService service) =>
            {
                var originalUrl = await service.GetOriginalUrlAsync(shortUrl);

                return originalUrl is null
                    ? Results.NotFound("URL not found.")
                    : Results.Ok(new { originalUrl });
            })
            .WithName("GetUrlShortener")
            .WithOpenApi();

            app.MapPost("/url-shortener", async (UrlShortenerRequestModel model, [FromServices] IUrlShortenerService service) =>
            {
                var shortUrl = await service.ShortenUrlAsync(model.Url);

                return Results.Created($"/url-shortener/{shortUrl}", new UrlShortenerResponseModel
                {
                    Url = model.Url,
                    ShortUrl = shortUrl,
                    CreatedAt = DateTime.UtcNow
                });
            })
            .WithName("PostUrlShortener")
            .WithOpenApi();
        }
    }
}
