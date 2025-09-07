namespace CommonLibrary;

using System.Text.Json.Serialization;

public class MovieResponse
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("results")]
    public List<Movie> Results { get; set; } = null!;

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }
}

public record Movie
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("original_language")]
    public string OriginalLanguage { get; set; } = null!;

    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; } = null!;

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
    
    [JsonPropertyName("original_title")]
    public string OriginalTitle { get; set; } = null!;
}
