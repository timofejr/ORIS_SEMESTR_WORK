using System.Net;
using MiniHttpServer.Framework.Core.HttpResponse;

namespace MiniHttpServer.Framework.Core.Abstracts;

public abstract class BaseController
{
    protected HttpListenerContext HttpContext { get; set; }

    public void SetContext(HttpListenerContext httpContext)
    {
        HttpContext = httpContext;
    }
    
    protected IResponseResult Page(string templatePath, object data = null, HttpStatusCode statusCode = HttpStatusCode.OK) 
        => new PageResult(templatePath, data, statusCode);
    protected IResponseResult Json(string jsonString, HttpStatusCode statusCode = HttpStatusCode.OK) 
        => new JsonResult(jsonString, statusCode);
}