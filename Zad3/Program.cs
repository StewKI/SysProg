using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Zad3;

var exitEvent = new ManualResetEvent(false);
Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    exitEvent.Set();
};

var server = new Server();
server.Start();

var subscription = server.RequestStream
    .SelectMany(
        context =>
            Observable.FromAsync(() => RequestHandler.HandleRequestAsync(context))
                      .SubscribeOn(TaskPoolScheduler.Default)
    )
    .Do( // logging
        _ => { }, // OnNext, nista dodatno
        ex => Logger.Log($"Pipeline error: {ex.Message}"), // OnError
        () => Logger.Log("Pipeline completed") // OnCompleted
    )
    .ObserveOn(NewThreadScheduler.Default)
    .Subscribe();

exitEvent.WaitOne();
subscription.Dispose();
server.Stop();


