using UrlShortener.Api.Endpoints.Models;

namespace UrlShortener.Api.Services.Abstractions
{
    public interface IUrlShortenerService
    {
        Task<string> ShortenUrlAsync(string url);
        Task<string?> GetOriginalUrlAsync(string shortUrl);

        Task<IEnumerable<UrlShortenerResponseModel>> GetAllUrlsAsync();
    }
}
