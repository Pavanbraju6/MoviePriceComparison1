using MoviePriceComparison.Interface;
using MoviePriceComparison.Model;
using Polly;
using System.Text.Json;
using Polly;
using Polly.Retry;

namespace MoviePriceComparison.Services
{
    public class FilmWorldService : IFilmWorldService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly AsyncRetryPolicy _retryPolicy;

        public FilmWorldService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["MovieApi:BaseUrl"]);
            _httpClient.DefaultRequestHeaders.Add("x-access-token", _configuration["MovieApi:ApiToken"]);
            // Initialize Polly retry policy
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(500 * retryAttempt));
        }

        public async Task<List<MovieSummary>> GetMoviesAsync()
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync("filmworld/movies");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MovieListResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Movies ?? new List<MovieSummary>();
            });
        }

        public async Task<MovieDetail> GetMovieDetailAsync(string id)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
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
            });
        }
    }

}
