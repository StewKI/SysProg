using System.Net;
using System.Reactive.Subjects;

namespace Zad3;

public class Server
{
    private const string Url = "http://localhost:8080/";

    private readonly HttpListener listener = new();
    private Task? listeningTask;

    public bool IsRunning { get; private set; } = false;

    private readonly Subject<HttpListenerContext> requestSubject = new();
    public IObservable<HttpListenerContext> RequestStream => requestSubject;

    public void Start()
    {
        if (IsRunning) return;
        IsRunning = true;

        listener.Prefixes.Add(Url);
        listener.Start();
        Logger.Log("Web server running on " + Url);

        listeningTask = ListeningAsync();
    }

    public void Stop()
    {
        if (!IsRunning) return;

        listener.Stop();
        listeningTask?.Wait();
        IsRunning = false;
        Logger.Log("Server stopped.");
    }

    private async Task ListeningAsync()
    {
        try
        {
            while (true)
            {
                var context = await listener.GetContextAsync();

                requestSubject.OnNext(context);
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
            requestSubject.OnCompleted();
            listener.Close();
        }
    }
}
