// See https://aka.ms/new-console-template for more information

using CommonLibrary;
using Zad1;

var server = new Server();
var exitEvent = new ManualResetEvent(false);

Console.CancelKeyPress += (s, e) =>
{
    Logger.Log("Stopping server...");
    
    if (server.IsRunning) server.Stop();
    
    exitEvent.Set();
    e.Cancel = true;
};

server.Start();

exitEvent.WaitOne();
Logger.Log("Exiting...");