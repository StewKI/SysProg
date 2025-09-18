using System.Net;
using System.Text.Json;
using CommonLibrary;

namespace Zad2;

public class TmdbApi
{
    private const string ApiKey = "cf1b53413c8e5acce75561edc234ee72";
    private const int RequestTimeout = 5; //seconds
    
    private readonly HttpClient client = new()
    {
        Timeout = TimeSpan.FromSeconds(RequestTimeout)
    };

    private TmdbApi() {}

    private static TmdbApi? instance = null;
    public static TmdbApi Instance => instance ??= new TmdbApi();

    public async Task<List<Movie>> GetMoviesAsync(string query)
    {
        var res = await client.GetAsync(GetUrl(query));
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync();
        
        var response = JsonSerializer.Deserialize<MovieResponse>(json);

        if (response is null)
        {
            throw new Exception("Invalid response - Deserialization failed");
        }

        return response.Results;
    }


    private string GetUrl(string query)
    {
        return $"https://api.themoviedb.org/3/search/movie?query={Uri.EscapeDataString(query)}&api_key={ApiKey}";
    }
}
