using Microsoft.AspNetCore.Mvc;
using MoviePriceComparison.Interface;

namespace MoviePriceComparison.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieComparisonService _comparisonService;

        public MoviesController(IMovieComparisonService comparisonService)
        {
            _comparisonService = comparisonService;
        }

        [HttpGet("cheapest")]
        public async Task<IActionResult> GetCheapestMovies()
        {
            var result = await _comparisonService.GetCheapestMoviePricesAsync();
            return Ok(result);
        }
    }

}
