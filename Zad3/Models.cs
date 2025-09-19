namespace Zad3;

using System.Text.Json.Serialization;

public class BusinessResult
{
    [JsonPropertyName("businesses")]
    public List<Restaurant> Restaurants { get; set; } = new();
}

public class Restaurant
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("review_count")]
    public int ReviewCount { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [JsonPropertyName("price")]
    public string Price { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public Location Location { get; set; } = new();

    [JsonPropertyName("display_phone")]
    public string DisplayPhone { get; set; } = string.Empty;

    [JsonPropertyName("distance")]
    public double Distance { get; set; }
}

public class Location
{
    [JsonPropertyName("display_address")]
    public List<string> DisplayAddress { get; set; } = new();
}