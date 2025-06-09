//using Microsoft.AspNetCore.Mvc;
//using MoviePriceComparison.Interface;

//namespace MoviePriceComparison.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TestController : ControllerBase
//    {
//        private readonly ICinemaworldService _cinemaworldService;

//        public TestController(ICinemaworldService cinemaworldService)
//        {
//            _cinemaworldService = cinemaworldService;
//        }

//        [HttpGet("movies")]
//        public async Task<IActionResult> GetMovies()
//        {
//            var json = await _cinemaworldService.GetMoviesJsonAsync();
//            return Content(json, "application/json");
//        }
//    }

//}
