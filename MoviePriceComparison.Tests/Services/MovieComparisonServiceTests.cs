using MoviePriceComparison.Interface;
using MoviePriceComparison.Model;
using MoviePriceComparison.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;


namespace MoviePriceComparison.Tests.Services
{
    public class MovieComparisonServiceTests
    {
        [Fact]
        public async Task GetCheapestMoviePricesAsync_ReturnsCheapestPricesFromBothProviders()
        {
            // Arrange
            var cinemaWorldMock = Substitute.For<ICinemaWorldService>();
            var filmWorldMock = Substitute.For<IFilmWorldService>();

            var cwMovies = new List<MovieSummary>
        {
            new MovieSummary { Title = "Test Movie", ID = "cw123", Poster = "cwPoster" }
        };
            var fwMovies = new List<MovieSummary>
        {
            new MovieSummary { Title = "Test Movie", ID = "fw123", Poster = "fwPoster" }
        };

            var cwDetail = new MovieDetail
            {
                Title = "Test Movie",
                ID = "cw123",
                Price = "15.00",
                Provider = "Cinemaworld"
            };

            var fwDetail = new MovieDetail
            {
                Title = "Test Movie",
                ID = "fw123",
                Price = "10.00",
                Provider = "Filmworld"
            };

            cinemaWorldMock.GetMoviesAsync().Returns(cwMovies);
            filmWorldMock.GetMoviesAsync().Returns(fwMovies);

            cinemaWorldMock.GetMovieDetailAsync("cw123").Returns(cwDetail);
            filmWorldMock.GetMovieDetailAsync("fw123").Returns(fwDetail);

            var service = new MovieComparisonService(cinemaWorldMock, filmWorldMock);

            // Act
            var result = await service.GetCheapestMoviePricesAsync();

            // Assert
            Assert.Single(result);
            var movie = result[0];
            Assert.Equal("Test Movie", movie.Title);
            Assert.Equal("Filmworld", movie.CheapestProvider);
            Assert.Equal(10.00m, movie.CheapestPrice);
            Assert.Equal("cwPoster", movie.Poster); // fallback to cw poster if available
        }
    }
}

    
