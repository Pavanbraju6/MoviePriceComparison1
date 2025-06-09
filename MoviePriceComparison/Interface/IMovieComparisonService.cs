using MoviePriceComparison.Model;

namespace MoviePriceComparison.Interface
{
    public interface IMovieComparisonService
    {
        Task<List<MovieComparisonResult>> GetCheapestMoviePricesAsync();
    }

}
