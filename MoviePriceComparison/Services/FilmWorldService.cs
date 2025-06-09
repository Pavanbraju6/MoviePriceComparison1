using MoviePriceComparison.Interface;
using MoviePriceComparison.Model;
using System.Text.Json;

namespace MoviePriceComparison.Services
{
    public class FilmWorldService : IFilmWorldService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FilmWorldService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["MovieApi:BaseUrl"]);
            _httpClient.DefaultRequestHeaders.Add("x-access-token", _configuration["MovieApi:ApiToken"]);
        }

        public async Task<List<MovieSummary>> GetMoviesAsync()
        {
            var response = await _httpClient.GetAsync("filmworld/movies");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MovieListResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Movies ?? new List<MovieSummary>();
        }

        public async Task<MovieDetail> GetMovieDetailAsync(string id)
        {
            var response = await _httpClient.GetAsync($"filmworld/movie/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MovieDetail>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result != null)
            {
                result.Provider = "Filmworld";
            }

            return result;
        }
    }

}
