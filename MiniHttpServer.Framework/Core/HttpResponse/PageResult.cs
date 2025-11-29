using System.Net;
using MiniHttpServer.Framework.Context;
using MiniTemplateEngine;

namespace MiniHttpServer.Framework.Core.HttpResponse;

public class PageResult: IResponseResult
{
    private string _templatePath;
    private object _data;
    
    public HttpStatusCode StatusCode { get; }
    
    public PageResult(string templatePath, object data, HttpStatusCode statusCode)
    {
        _templatePath = GlobalContext.SettingsManager.Settings.StaticFilesPath + templatePath;
        _data = data;
        StatusCode = statusCode;
    }
    
    public string Execute(HttpListenerContext httpContext)
    {
        var templateRenderer = new HtmlTemplateRenderer();

        var renderedPage = templateRenderer.RenderFromFile(_templatePath, _data);
        
        return renderedPage;
    }

}