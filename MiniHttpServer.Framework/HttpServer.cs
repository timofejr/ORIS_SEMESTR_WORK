using System.Net;
using System.Text.Json.Nodes;
using MiniHttpServer.Framework.Context;
using MiniHttpServer.Framework.Core.Handlers;
using MiniHttpServer.Framework.Utils;

namespace MiniHttpServer.Framework;

public class HttpServer
{
    private readonly HttpListener _listener = new ();

    public void Start()
    {
        _listener.Prefixes.Add($"http://{GlobalContext.SettingsManager.Settings.Domain}:{GlobalContext.SettingsManager.Settings.Port}/");
        _listener.Start();
        GlobalContext.Logger.ServerStarted(GlobalContext.SettingsManager.Settings.Domain, GlobalContext.SettingsManager.Settings.Port);
        Receive();
    }

    public void Stop()
    {
        _listener.Stop();
    }

    private void Receive()
    {
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
    }

    private void ListenerCallback(IAsyncResult result)
    {

        if (!_listener.IsListening) return;
        var context = _listener.EndGetContext(result);
        
        var staticFilesHandler = new StaticFilesHandler();
        var controllerHandler = new ControllersHandler();
        staticFilesHandler.Successor = controllerHandler;
        staticFilesHandler.HandleRequest(context);

        Receive();
    }
    
    public void SendStaticResponse(HttpListenerContext context, HttpStatusCode statusCode, string path)
    {
        var response = context.Response;
        var request = context.Request;
        
        response.StatusCode = (int)statusCode;
        response.ContentType = ContentType.GetContentTypeFromFile(path);

        var buffer = BufferManager.GetBytesFromFile(path);
        response.ContentLength64 = buffer.Length;

        using var output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        
        
        if (response.StatusCode == 200)
            GlobalContext.Logger.LogMessage($"Запрос обработан: {request.Url.AbsolutePath} {request.HttpMethod} - Status: {response.StatusCode}");
        else
            GlobalContext.Logger.LogMessage($"Ошибка запроса: {request.Url.AbsolutePath} {request.HttpMethod} - Status: {response.StatusCode}");
    }

    public void SendPageResponse(HttpListenerContext context, HttpStatusCode statusCode, string renderedPage)
    {
        var response = context.Response;
        var request = context.Request;
        
        response.StatusCode = (int)statusCode;
        response.ContentType = ContentType.GetContentTypeByExtension(".html");
        
        var buffer = BufferManager.GetBytesFromString(renderedPage);
        response.ContentLength64 = buffer.Length;

            
        using var output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        
        if (response.StatusCode == 200)
            GlobalContext.Logger.LogMessage($"Запрос обработан: {request.Url.AbsolutePath} {request.HttpMethod} - Status: {response.StatusCode}");
        else
            GlobalContext.Logger.LogMessage($"Ошибка запроса: {request.Url.AbsolutePath} {request.HttpMethod} - Status: {response.StatusCode}");
    }

    public void SendJsonResponse(HttpListenerContext context, HttpStatusCode statusCode, string jsonString = "")
    {
        var response = context.Response;
        var request = context.Request;
        
        response.StatusCode = (int)statusCode;
        response.ContentType = ContentType.GetContentTypeByExtension(".json");

        var buffer = BufferManager.GetBytesFromString(jsonString);
        response.ContentLength64 = buffer.Length;
        // response.Acce

        using var output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        
        
        if (response.StatusCode == 200)
            GlobalContext.Logger.LogMessage($"Запрос обработан: {request.Url.AbsolutePath} {request.HttpMethod} - Status: {response.StatusCode}");
        else
            GlobalContext.Logger.LogMessage($"Ошибка запроса: {request.Url.AbsolutePath} {request.HttpMethod} - Status: {response.StatusCode}");
    }

    public void Send404Response(HttpListenerContext context, string path)
    {
        try
        {
            var path404 = GlobalContext.SettingsManager.Settings.StaticFilesPath + path.Split('/')[1] + "/404.html";

            if (File.Exists(path404))
            {
                GlobalContext.Server.SendStaticResponse(context, HttpStatusCode.NotFound, path404);
            }
            else
            {
                GlobalContext.Server.SendStaticResponse(context, HttpStatusCode.NotFound,
                    GlobalContext.SettingsManager.Settings.StaticFilesPath + "/404.html");
            }
        }
        catch (Exception ex)
        {
            GlobalContext.Server.SendStaticResponse(context, HttpStatusCode.NotFound,
                GlobalContext.SettingsManager.Settings.StaticFilesPath + "/404.html");
        }
        finally
        {
            if (context.Response.StatusCode == 200)
                GlobalContext.Logger.LogMessage($"Запрос обработан: {context.Request.Url.AbsolutePath} {context.Request.HttpMethod} - Status: {context.Response.StatusCode}");
            else
                GlobalContext.Logger.LogMessage($"Ошибка запроса: {context.Request.Url.AbsolutePath} {context.Request.HttpMethod} - Status: {context.Response.StatusCode}");

        }
    }
}
