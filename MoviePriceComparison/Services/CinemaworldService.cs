
//using System.Net.Http.Headers;
//using Microsoft.Extensions.Options;

//public class MovieApiOptions
//{
//    public string BaseUrl { get; set; }
//    public string ApiToken { get; set; }
//}

//public interface ICinemaworldService
//{
//    Task<string> GetMoviesJsonAsync();
//}

//public class CinemaworldService : ICinemaworldService
//{
//    private readonly HttpClient _httpClient;
//    private readonly MovieApiOptions _options;

//    public CinemaworldService(HttpClient httpClient, IOptions<MovieApiOptions> options)
//    {
//        _httpClient = httpClient;
//        _options = options.Value;
//    }

//    public async Task<string> GetMoviesJsonAsync()
//    {
//        try
//        {
//            _httpClient.DefaultRequestHeaders.Remove("x-access-token");
//            _httpClient.DefaultRequestHeaders.Add("x-access-token", _options.ApiToken);

//            var response = await _httpClient.GetAsync($"{_options.BaseUrl}/movies");

//            response.EnsureSuccessStatusCode(); // throws on 503
//            return await response.Content.ReadAsStringAsync();
//        }
//        catch (HttpRequestException ex)
//        {
//            Console.WriteLine("API error: " + ex.Message);
//            return "{\"error\":\"Service unavailable or failed to fetch data.\"}";
//        }
//    }

//}



using MoviePriceComparison.Interface;
using MoviePriceComparison.Model;
using System.Text.Json;

public class CinemaWorldService : ICinemaWorldService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CinemaWorldService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(_configuration["MovieApi:BaseUrl"]);
        _httpClient.DefaultRequestHeaders.Add("x-access-token", _configuration["MovieApi:ApiToken"]);
    }

    public async Task<List<MovieSummary>> GetMoviesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("cinemaworld/movies");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MovieListResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Movies ?? new List<MovieSummary>();
        }
        catch (Exception ex)
        {
            // Log the error (you can inject ILogger)
            Console.WriteLine($"CinemaWorldService.GetMoviesAsync failed: {ex.Message}");
            return new List<MovieSummary>(); // Return empty list to allow app to continue
        }
        
    }

    public async Task<MovieDetail> GetMovieDetailAsync(string id)
    {
        var response = await _httpClient.GetAsync($"cinemaworld/movie/{id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<MovieDetail>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result != null)
        {
            result.Provider = "Cinemaworld";
        }

        return result;
    }
}
