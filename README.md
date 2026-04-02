# Requiriments to run
- Dotnet core sdk 8.0
- Visual Studio 2022

## Running locally
- Add enviroment variable for sercret key:

  ```setx UrlShortener_SecretKey "my-super-secret-security-key-2026"```

## Api Project
- Made using Minimal APIs, Dependency injection and storing URLs with InMemoryCache class.
- UrlShortener/src/UrlShortener.Api

  Contains APIs for GetAllUrls, GetByShortUrl and PostUrl.
  See Swagger documentation.


## Tests Project
- Made with xUnit.
- UrlShortener/src/UrlShortener.Tests

  Contains Tests for methods GetAllUrls, GetByShortUrl and PostUrl from UrlShortenerService class.
