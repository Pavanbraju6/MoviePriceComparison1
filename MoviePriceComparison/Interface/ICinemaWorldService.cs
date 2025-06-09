using MoviePriceComparison.Model;

namespace MoviePriceComparison.Interface
{
    public interface ICinemaWorldService
    {
        Task<List<MovieSummary>> GetMoviesAsync();
        Task<MovieDetail> GetMovieDetailAsync(string id);
    }

}
