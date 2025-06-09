

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;


namespace MoviePriceComparison.Tests.Services
{
    public class CinemaWorldServiceTests
    {
        [Fact]
        public async Task GetMoviesAsync_ReturnsMoviesList()
        {
            // Arrange
            var movieJson = "{\"Movies\":[{\"Title\":\"Test Movie\",\"Year\":\"2020\",\"ID\":\"cw123\",\"Type\":\"movie\",\"Poster\":\"url\"}]}";

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

            var service = new CinemaWorldService(httpClient, mockConfig);

            // Act
            var result = await service.GetMoviesAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Test Movie", result[0].Title);
        }
    }

}

