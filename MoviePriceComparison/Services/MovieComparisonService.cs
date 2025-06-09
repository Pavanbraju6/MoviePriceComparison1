using MoviePriceComparison.Interface;
using MoviePriceComparison.Model;

namespace MoviePriceComparison.Services
{
    public class MovieComparisonService : IMovieComparisonService
    {
        private readonly ICinemaWorldService _cinemaWorld;
        private readonly IFilmWorldService _filmWorld;

        public MovieComparisonService(ICinemaWorldService cinemaWorld, IFilmWorldService filmWorld)
        {
            _cinemaWorld = cinemaWorld;
            _filmWorld = filmWorld;
        }

        public async Task<List<MovieComparisonResult>> GetCheapestMoviePricesAsync()
        {
            var results = new List<MovieComparisonResult>();

            var cwMovies = await _cinemaWorld.GetMoviesAsync();
            var fwMovies = await _filmWorld.GetMoviesAsync();

            var uniqueTitles = cwMovies.Select(m => m.Title).Union(fwMovies.Select(m => m.Title));

            foreach (var title in uniqueTitles)
            {
                var cwMovie = cwMovies.FirstOrDefault(m => m.Title == title);
                var fwMovie = fwMovies.FirstOrDefault(m => m.Title == title);

                MovieDetail cwDetail = null;
                MovieDetail fwDetail = null;

                if (cwMovie != null)
                    cwDetail = await _cinemaWorld.GetMovieDetailAsync(cwMovie.ID);

                if (fwMovie != null)
                    fwDetail = await _filmWorld.GetMovieDetailAsync(fwMovie.ID);

                if (cwDetail == null && fwDetail == null)
                    continue;

                var cheapest = new MovieComparisonResult
                {
                    Title = title,
                    Poster = cwMovie?.Poster ?? fwMovie?.Poster,
                    CheapestProvider = (cwDetail != null && (fwDetail == null || decimal.Parse(cwDetail.Price) < decimal.Parse(fwDetail.Price)))
                        ? "Cinemaworld"
                        : "Filmworld",
                    CheapestPrice = Math.Min(
                        cwDetail != null ? decimal.Parse(cwDetail.Price) : decimal.MaxValue,
                        fwDetail != null ? decimal.Parse(fwDetail.Price) : decimal.MaxValue)
                };

                results.Add(cheapest);
            }

            return results;
        }
    }

}
