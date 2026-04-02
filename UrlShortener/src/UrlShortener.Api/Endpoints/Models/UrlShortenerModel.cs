namespace UrlShortener.Api.Endpoints.Models
{
    public record UrlShortenerRequestModel(string Url);

    public record UrlShortenerResponseModel
    {
        public string Url { get; init; } = string.Empty;
        public string ShortUrl { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
