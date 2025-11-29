namespace MiniHttpServer.Framework.Core.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class HttpGet : Attribute
{
    public string? Route { get; }
    public bool RouteParameter { get; }

    public HttpGet()
    {
    }

    public HttpGet(string? route, bool routeParameter = false)
    {
        Route = route;
        RouteParameter = routeParameter;
    }
}