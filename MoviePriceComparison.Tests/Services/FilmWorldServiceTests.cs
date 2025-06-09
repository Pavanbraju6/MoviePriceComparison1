using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using MoviePriceComparison.Services;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace MoviePriceComparison.Tests.Services
{
    public class FilmWorldServiceTests
    {
        [Fact]
        public async Task GetMoviesAsync_ReturnsMoviesList()
        {
            // Arrange
            var movieJson = "{\"Movies\":[{\"Title\":\"FilmWorld Movie\",\"Year\":\"2021\",\"ID\":\"fw123\",\"Type\":\"movie\",\"Poster\":\"url\"}]}";

            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(movieJson),
               });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://webjetapitest.azurewebsites.net/api/")
            };

            var mockConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                { "MovieApi:BaseUrl", "https://webjetapitest.azurewebsites.net/api/" },
                { "MovieApi:Token", "fake-token" }
                }).Build();

            var service = new FilmWorldService(httpClient, mockConfig);

            // Act
            var result = await service.GetMoviesAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("FilmWorld Movie", result[0].Title);
        }

        [Fact]
        public async Task GetMovieDetailAsync_ReturnsMovieDetailWithProvider()
        {
            // Arrange
            var movieDetailJson = @"{
        ""Title"": ""Test Movie Detail"",
        ""ID"": ""fw001"",
        ""Type"": ""movie"",
        ""Poster"": ""url""
    }";

            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync",
                   ItExpr.Is<HttpRequestMessage>(req =>
                       req.Method == HttpMethod.Get &&
                       req.RequestUri!.ToString().Contains("filmworld/movie/fw001")),
                   ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(movieDetailJson),
               });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://webjetapitest.azurewebsites.net/api/")
            };

            var mockConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
            { "MovieApi:BaseUrl", "https://webjetapitest.azurewebsites.net/api/" },
            { "MovieApi:Token", "fake-token" }
                }).Build();

            var service = new FilmWorldService(httpClient, mockConfig);

            // Act
            var result = await service.GetMovieDetailAsync("fw001");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Movie Detail", result.Title);
            Assert.Equal("fw001", result.ID);
            Assert.Equal("Filmworld", result.Provider);
        }

    }

}
