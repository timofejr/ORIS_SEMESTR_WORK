using System.Net;

namespace MiniHttpServer.Framework.Core.HttpResponse;

public class JsonResult: IResponseResult
{
    private readonly string _jsonString;
    public HttpStatusCode StatusCode { get; }
    
    public JsonResult(string jsonString, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        _jsonString = jsonString;
        StatusCode = statusCode;
    }
    
    public string Execute(HttpListenerContext httpContext) => _jsonString;
}