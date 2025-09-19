using System.Net;
using System.Text.Json;

namespace Zad3;

public class YelpApi
{
    private const string ApiKey = "qZkLgjFh7VT7PlMt4d101x3OgU1mFr4dwrKRwkP6yhmkeBzUWHRJE3E8Brd2dIXWBj-70k9YzAeMok3Oq6RoERaVJu7gqHjAmHC1nhLbf6xGJcuDc4AmY5sjtgTMaHYx";
    private const int RequestTimeout = 5; //seconds

    private readonly HttpClient http = new()
    {
        Timeout = TimeSpan.FromSeconds(RequestTimeout)
    };

    private YelpApi()
    {
        http.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
    }
    private static YelpApi? instance = null;
    public static YelpApi Instance => instance ??= new YelpApi();

    public async Task<List<Restaurant>> GetRestaurantsAsync(string location)
    {
        var response = await http.GetAsync(GetUrl(location));
        
        if (response.StatusCode == HttpStatusCode.BadRequest) // when location is not found
        {
            throw new LocationNotFoundException();
        }

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        
        var result = JsonSerializer.Deserialize<BusinessResult>(json);

        if (result is null)
        {
            throw new Exception("Invalid response - Deserialization failed");
        }

        return result.Restaurants;
    }

    private string GetUrl(string location)
    {
        return $"https://api.yelp.com/v3/businesses/search?location={Uri.EscapeDataString(location)}&open_now=true&limit=50";
    }

    public class LocationNotFoundException : Exception
    {
        public LocationNotFoundException() : base("Lokacija nije pronadjena") { }
    }
}