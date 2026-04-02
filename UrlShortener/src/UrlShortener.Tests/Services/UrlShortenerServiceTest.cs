using Microsoft.Extensions.Caching.Memory;
using UrlShortener.Api.Services.Core;

namespace UrlShortener.Tests.Services
{
    public class UrlShortenerServiceTest
    {
        private readonly IMemoryCache _cache;
        private readonly UrlShortenerService _service;
        private const string SecretKey = "test-secret-key";

        public UrlShortenerServiceTest()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _service = new UrlShortenerService(_cache, SecretKey);
        }

        [Fact]
        public async Task ShortenUrlAsync_ShouldReturnShortUrl()
        {
            // Arrange
            var url = "https://www.google.com";

            // Act
            var shortUrl = await _service.ShortenUrlAsync(url);

            // Assert
            Assert.NotNull(shortUrl);
            Assert.Equal(7, shortUrl.Length);
        }

        [Fact]
        public async Task ShortenUrlAsync_SameUrl_ShouldAlwaysReturnSameShortUrl()
        {
            // Arrange
            var url = "https://www.google.com";

            // Act
            var shortUrl1 = await _service.ShortenUrlAsync(url);
            var shortUrl2 = await _service.ShortenUrlAsync(url);

            // Assert
            Assert.Equal(shortUrl1, shortUrl2);
        }

        [Fact]
        public async Task ShortenUrlAsync_DifferentUrls_ShouldReturnDifferentShortUrls()
        {
            // Arrange
            var url1 = "https://www.google.com";
            var url2 = "https://www.github.com";

            // Act
            var shortUrl1 = await _service.ShortenUrlAsync(url1);
            var shortUrl2 = await _service.ShortenUrlAsync(url2);

            // Assert
            Assert.NotEqual(shortUrl1, shortUrl2);
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldReturnOriginalUrl()
        {
            // Arrange
            var url = "https://www.google.com";
            var shortUrl = await _service.ShortenUrlAsync(url);

            // Act
            var result = await _service.GetOriginalUrlAsync(shortUrl);

            // Assert
            Assert.Equal(url, result);
        }

        [Fact]
        public async Task GetOriginalUrlAsync_InvalidShortUrl_ShouldReturnNotFound()
        {
            // Act
            var result = await _service.GetOriginalUrlAsync("invalid");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllUrlsAsync_ShouldReturnAllUrls()
        {
            // Arrange
            await _service.ShortenUrlAsync("https://www.google.com");
            await _service.ShortenUrlAsync("https://www.github.com");
            await _service.ShortenUrlAsync("https://www.stackoverflow.com");

            // Act
            var result = await _service.GetAllUrlsAsync();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllUrlsAsync_EmptyCache_ShouldReturnEmptyList()
        {
            // Act
            var result = await _service.GetAllUrlsAsync();

            // Assert
            Assert.Empty(result);
        }
    }
}
