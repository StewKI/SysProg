using System.Net;
using System.Text.Json;
using CommonLibrary;

namespace Zad1;

public class TmdbApi
{
    private const string ApiKey = "cf1b53413c8e5acce75561edc234ee72";
    private const int RequestTimeout = 5000; //5s

    private TmdbApi()
    {
    }

    private static TmdbApi? instance = null;

    public static TmdbApi Instance
    {
        get
        {
            if (instance is null)
            {
                instance = new TmdbApi();
            }
        
            return instance;
        }
    }


    public List<Movie> GetMovies(string query)
    {
        var json = FetchJsonFromUrl(GetUrl(query));
        var response = JsonSerializer.Deserialize<MovieResponse>(json);

        if (response is null)
        {
            throw new Exception("Invalid response - Deserialization failed");
        }

        return response.Results;
    }
    
    
    private string FetchJsonFromUrl(string url)
    {
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Timeout = RequestTimeout;

        using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using Stream stream = response.GetResponseStream();
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }


    private string GetUrl(string query)
    {
        return $"https://api.themoviedb.org/3/search/movie?query={Uri.EscapeDataString(query)}&api_key={ApiKey}";
    }
}
