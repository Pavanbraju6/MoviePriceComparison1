using MoviePriceComparison.Model;

namespace MoviePriceComparison.Interface
{
    public interface IFilmWorldService
    {
        Task<List<MovieSummary>> GetMoviesAsync();
        Task<MovieDetail> GetMovieDetailAsync(string id);
    }

}
