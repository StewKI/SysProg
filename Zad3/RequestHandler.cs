using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;

namespace Zad3;

internal static class RequestHandler
{
    public static async Task HandleRequestAsync(HttpListenerContext context)
    {
        string? location = context.Request.QueryString["location"];

        if (location is null)
        {
            string html = WebFormatter.FormatMessagePage("Nije unesena lokacija kao query parametar");
            await WriteHtmlResponse(context, html);
            Logger.LogRequest(false, location, context);
            return;
        }

        try
        {
            var restaurants =
                await YelpApi.Instance.GetRestaurantsAsync(location)
                .ToObservable()
                .FilterAndSort();

            string html = WebFormatter.FormatRestaurantsPage(restaurants);
            await WriteHtmlResponse(context, html);

            Logger.LogRequest(true, location, context);
        }
        catch (YelpApi.LocationNotFoundException)
        {
            string html = WebFormatter.FormatMessagePage("Lokacija nije pronadjena");
            await WriteHtmlResponse(context, html);
            Logger.LogRequest(false, location, context, "Lokacija nije pronadjena");
        }
        catch (Exception ex)
        {
            string html = WebFormatter.FormatMessagePage("Greška na serveru");
            await WriteHtmlResponse(context, html);
            Logger.LogRequest(false, location, context, ex.Message);
        }
    }

    private static async Task WriteHtmlResponse(HttpListenerContext context, string html)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        byte[] buffer = Encoding.UTF8.GetBytes(html);
        context.Response.ContentLength64 = buffer.Length;
        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        context.Response.Close();
    }
}
