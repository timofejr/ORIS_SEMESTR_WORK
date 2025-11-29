using System;

namespace MiniHttpServer.Utils;

public class Logger
{
    private static Logger _instance;
    private static Lock _lock = new ();
    
    private Logger()
    {
    }
    
    public static Logger Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new Logger();
                }
            }
            return _instance;
        }
    }
    
    public void LogMessage(string message)
    {
        var now = DateTime.Now;
        
        Console.WriteLine($"{now.ToString("dd.MM.yyyy hh:mm:ss")}: {message}");
    }

    public void ServerStarted(string domain, string port)
    {
        var now = DateTime.Now;
        
        Console.WriteLine($"{now.ToString("dd.MM.yyyy hh:mm:ss")}: Сервер запустился: http://{domain}:{port}/");
    }

    public void ServerStopped()
    {
        var now = DateTime.Now;

        Console.WriteLine($"{now.ToString("dd.MM.yyyy hh:mm:ss")}: Сервер завершил работу");
    }
}   
