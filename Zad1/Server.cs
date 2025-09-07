using System.Net;
using System.Text;
using CommonLibrary;

namespace Zad1;

public class Server
{
    private const string Url = "http://localhost:8080/";

    private readonly HttpListener listener = new();
    private Thread? listeningThread;
    private readonly ApiRequestQueue apiQueue;

    private bool running = false;
    public bool IsRunning => running;

    public Server()
    {
        int workerCount = Math.Clamp(Environment.ProcessorCount, 4, 8); // between 4 and 8 threads, depending on the machine
        apiQueue = new ApiRequestQueue(workerCount);
    }

    public void Start()
    {
        if (running) return;
        running = true;

        listener.Prefixes.Add(Url);
        listener.Start();
        Logger.Log("Web server running on " + Url);

        listeningThread = new Thread(Listening) { IsBackground = true };
        listeningThread.Start();
    }

    public void Stop()
    {
        if (!running) return;

        apiQueue.Stop();
        listener.Stop();
        listeningThread?.Join();
        running = false;
        Logger.Log("Server stopped.");
    }

    private void Listening()
    {
        try
        {
            while (true)
            {
                var context = listener.GetContext();
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    var handler = new RequestHandler(context, apiQueue);
                    handler.Handle();
                });
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