using System.Net;
using System.Collections.Generic;
using System.Threading;
using CommonLibrary;

namespace Zad1;

public class ApiRequestQueue
{
    private readonly Queue<(string query, string cacheKey, HttpListenerContext context)> queue = new();
    private readonly Thread[] workers;
    private readonly Lock lockObj = new();
    private readonly SemaphoreSlim semaphore = new(0);
    private readonly int workerCount;
    private bool running = true;

    public ApiRequestQueue(int workerCount = 4)
    {
        this.workerCount = workerCount;
        workers = new Thread[this.workerCount];
        for (int i = 0; i < this.workerCount; i++)
        {
            workers[i] = new Thread(WorkerLoop) { IsBackground = true };
            workers[i].Start();
        }
    }

    public void Enqueue(string query, string cacheKey, HttpListenerContext context)
    {
        lock (lockObj)
        {
            queue.Enqueue((query, cacheKey, context));
        }

        semaphore.Release();
    }

    public void Stop()
    {
        running = false;

        for (int i = 0; i < workerCount; i++)
        {
            semaphore.Release();
        }

        foreach (var worker in workers)
        {
            worker.Join();
        }
    }

    private void WorkerLoop()
    {
        while (true)
        {
            semaphore.Wait();

            if (!running)
            {
                lock (lockObj)
                {
                    if (queue.Count == 0)
                        return;
                }
            }

            (string query, string cacheKey, HttpListenerContext context) workItem;

            lock (lockObj)
            {
                if (queue.Count == 0) continue;
                workItem = queue.Dequeue();
            }

            try
            {
                var movies = TmdbApi.Instance.GetMovies(workItem.query);
                Cache.Instance.AddOrUpdate(workItem.cacheKey, movies);

                var html = WebFormatter.FormatMoviesPage(movies);
                RequestHandler.SendResponse(workItem.context, html, 200);

                Logger.LogRequest(true, workItem.query, workItem.context);
            }
            catch (Exception ex)
            {
                var html = WebFormatter.FormatMessagePage("Greska na serveru");
                RequestHandler.SendResponse(workItem.context, html, 500);
                Logger.LogRequest(false, workItem.query, workItem.context, ex.Message);
            }
        }
    }
}
