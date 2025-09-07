using System.Text;

namespace CommonLibrary;

public static class WebFormatter
{
    public static string FormatMessagePage(string message)
    {
        return FormatPage($"<h1>{message}</h1>");
    }
    
    public static string FormatMoviesPage(List<Movie> movies)
    {
        StringBuilder sb = new();
        foreach (var movie in movies)
        {
            sb.Append(FormatMovie(movie));
        }

        if (movies.Count == 0)
        {
            sb.Append("<h1>Nije nadjen nijedan film</h1>");
        }
        
        return FormatPage(sb.ToString());
    }

    public static string FormatMovie(Movie movie)
    {
        return $"""
                <div class="movie-card">
                    <h2 class="movie-title">{movie.Title}</h2>
                    <p><strong>Originalno:</strong> {movie.OriginalTitle}</p>
                    <p><strong>Jezik:</strong> {movie.OriginalLanguage}</p>
                    <p><strong>Prva premijera:</strong> {movie.ReleaseDate}</p>
                    <p><strong>ID:</strong> {movie.Id}</p>
                </div>
                """;
    }

    public static string FormatPage(string body)
    {
        return $"""
               <!DOCTYPE html>
               <html>
               <head>
                   <title>Movie Search</title>
                   <style>{GetCss()}</style>
               </head>
               <body>
                   {body}
               </body>
               </html>
               """;
    }

    private static string GetCss()
    {
        return """
               body {
                   font-family: Arial, sans-serif;
                   background-color: #f4f4f9;
                   margin: 0;
                   padding: 20px;
               }
               
               h1 {
                   text-align: center;
               }
               
               .movie-card {
                   background: white;
                   border-radius: 10px;
                   padding: 15px 20px;
                   margin: 15px auto;
                   max-width: 500px;
                   box-shadow: 0 4px 10px rgba(0,0,0,0.1);
                   transition: transform 0.2s ease, box-shadow 0.2s ease;
               }
               
               .movie-card:hover {
                   transform: translateY(-5px);
                   box-shadow: 0 8px 20px rgba(0,0,0,0.15);
               }
               
               .movie-title {
                   font-size: 1.5rem;
                   margin-bottom: 10px;
                   color: #333;
               }
               
               p {
                   margin: 5px 0;
                   color: #555;
               }
               
               strong {
                   color: #222;
               }
               """;
    }
}