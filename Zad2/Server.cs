using System.Net;
using CommonLibrary;

namespace Zad2;

public class Server
{
    private const string Url = "http://localhost:8080/";

    private readonly HttpListener listener = new();
    private Task? listeningTask;

    private bool running = false;
    public bool IsRunning => running;

    public Server()
    {
    }

    public void Start()
    {
        if (running) return;
        running = true;

        listener.Prefixes.Add(Url);
        listener.Start();
        Logger.Log("Web server running on " + Url);

        listeningTask = ListeningAsync();
    }

    public void Stop()
    {
        if (!running) return;

        listener.Stop();
        listeningTask?.Wait();
        running = false;
        Logger.Log("Server stopped.");
    }

    private async Task ListeningAsync()
    {
        try
        {
            while (true)
            {
                var context = await listener.GetContextAsync();
                
                var handler = new RequestHandler(context);
                _ = handler.HandleAsync();
            }
        }
        catch (HttpListenerException)
        {
            Logger.Log("Listening thread stopped");
        }
        catch (Exception ex)
        {
            Logger.Log("***** FATAL ***** Error in listening thread, crashing... Details: " + ex.Message);
        }
        finally
        {
            listener.Close();
        }
    }
}