using System.Net;

namespace CommonLibrary;

public static class Logger
{
    public static void LogRequest(bool success, string? query, HttpListenerContext context, string? additionalInfo = null)
    {
        LogSuccess($"{context.Request.RawUrl} -> {query}   {additionalInfo}", success);
    }
    
    private static void LogSuccess(string message, bool success)
    {
        string status = success ? "USPESNO" : "NEUSPESNO";
        Log($"[{status}] {message}");
    }

    public static void Log(string message)
    {
        Console.WriteLine($"[{DateTime.Now}] {message}");
    }
}