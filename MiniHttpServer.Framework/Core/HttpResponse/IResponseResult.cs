using System.Net;

namespace MiniHttpServer.Framework.Core.HttpResponse;

public interface IResponseResult
{
    public HttpStatusCode StatusCode { get; }
    public string Execute(HttpListenerContext httpContext);
}