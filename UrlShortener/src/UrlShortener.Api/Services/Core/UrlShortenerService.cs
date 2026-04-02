using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UrlShortener.Api.Endpoints.Models;
using UrlShortener.Api.Services.Abstractions;

namespace UrlShortener.Api.Services.Core
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IMemoryCache _cache;
        private readonly string _secretKey;
        private const string _cacheKey = "url-shortener-list";

        public UrlShortenerService(IMemoryCache cache, string secretKey)
        {
            _cache = cache;
            _secretKey = secretKey;
        }

        public Task<IEnumerable<UrlShortenerResponseModel>> GetAllUrlsAsync()
        {
            var list = GetList();
            return Task.FromResult(list.Values.AsEnumerable());
        }

        public Task<string?> GetOriginalUrlAsync(string shortUrl)
        {
            var list = GetList();
            try
            {
                var model = list[shortUrl] ?? null;

                return Task.FromResult(model?.Url ?? "URL not found.");
            }
            catch (Exception ex)
            {
                return Task.FromResult<string?>(null);
            }
        }

        public Task<string> ShortenUrlAsync(string url)
        {
            var shortUrl = GenerateHash(url);

            var list = GetList();
            list[shortUrl] = new UrlShortenerResponseModel() { Url = url, ShortUrl = shortUrl, CreatedAt = DateTime.UtcNow };
            _cache.Set(_cacheKey, list, TimeSpan.FromHours(24));

            return Task.FromResult(shortUrl);
        }

        private string GenerateHash(string url)
        {
            var input = Encoding.UTF8.GetBytes(url + _secretKey);
            var hash = HMACSHA256.HashData(Encoding.UTF8.GetBytes(_secretKey), input);

            return Convert.ToBase64String(hash)
                .Replace("/", "_")
                .Replace("+", "-")
                .Replace("=", "")
                [..7];
        }

        private Dictionary<string, UrlShortenerResponseModel> GetList()
        {
            return _cache.GetOrCreate(_secretKey, _ =>
                new Dictionary<string, UrlShortenerResponseModel>())!;
        }
    }
}
