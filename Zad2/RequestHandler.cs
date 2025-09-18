using System.Net;
using System.Text;
using CommonLibrary;

namespace Zad2;

class RequestHandler
{
    private readonly HttpListenerContext context;

    public RequestHandler(HttpListenerContext context)
    {
        this.context = context;
    }

    public async Task HandleAsync()
    {
        string? query = context.Request.QueryString["query"];

        if (string.IsNullOrWhiteSpace(query))
        {
            RespondWithMessage("Niste uneli upit", 400);
            Logger.LogRequest(false, query, context);
            return;
        }

        string cacheKey = query.ToLower().Trim();
        var cache = Cache.Instance;

        if (cache.TryGet(cacheKey, out var moviesFromCache))
        {
            RespondWithMovies(moviesFromCache);
            Logger.LogRequest(true, query + " [CACHE]", context);
            return;
        }
        
        try
        {
            var movies = await TmdbApi.Instance.GetMoviesAsync(query);
            Cache.Instance.AddOrUpdate(cacheKey, movies);

            var html = WebFormatter.FormatMoviesPage(movies);
            SendResponse(context, html, 200);

            Logger.LogRequest(true, query, context);
        }
        catch (Exception ex)
        {
            var html = WebFormatter.FormatMessagePage("Greska na serveru");
            SendResponse(context, html, 500);
            Logger.LogRequest(false, query, context, ex.Message);
        }
    }

    private void RespondWithMessage(string message, int statusCode)
    {
        var html = WebFormatter.FormatMessagePage(message);
        SendResponse(context, html, statusCode);
    }

    private void RespondWithMovies(List<Movie> movies)
    {
        var html = WebFormatter.FormatMoviesPage(movies);
        SendResponse(context, html, 200);
    }

    public static void SendResponse(HttpListenerContext context, string content, int statusCode)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(content);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "text/html; charset=utf-8";
        context.Response.ContentLength64 = buffer.Length;
        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        context.Response.Close();
    }
}