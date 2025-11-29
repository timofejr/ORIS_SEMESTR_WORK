// EndpointsRegistry.cs
using System.Reflection;
using MiniHttpServer.Framework.Core.Attributes;
using System.Text.RegularExpressions;

namespace MiniHttpServer.Framework.Utils;

public static class EndpointsRegistry
{
    public static Dictionary<(Regex RoutePattern, string HttpMethod), (Type Type, MethodInfo Method, bool HasRouteParameter)> LoadEndpoints(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(t => t.GetCustomAttribute<Controller>() != null)
            .SelectMany(type => type
                .GetMethods()
                .SelectMany(method => GetHttpMappings(type, method)))
            .ToDictionary(
                x => (CreateRoutePattern(x.Route), x.HttpMethod),
                x => (x.Type, x.Method, x.HasRouteParameter));
    }

    private static Regex CreateRoutePattern(string route)
    {
        if (string.IsNullOrEmpty(route))
            throw new ArgumentException("Route cannot be null or empty");

        string cleanedRoute = route.Trim('/');
        string pattern = Regex.Replace(cleanedRoute, @"\{[^{}]+\}", "([0-9]+)");
        return new Regex($"^{pattern}$");
    }

    private static IEnumerable<(string Route, string HttpMethod, Type Type, MethodInfo Method, bool HasRouteParameter)> GetHttpMappings(Type type, MethodInfo method)
    {
        var get = method.GetCustomAttribute<HttpGet>();
        if (get != null)
        {
            if (string.IsNullOrEmpty(get.Route))
                throw new ArgumentException($"Route cannot be null or empty for method {method.Name}");
            yield return (get.Route, "GET", type, method, get.RouteParameter);
        }

        var post = method.GetCustomAttribute<HttpPost>();
        if (post != null)
        {
            if (string.IsNullOrEmpty(post.Route))
                throw new ArgumentException($"Route cannot be null or empty for method {method.Name}");
            yield return (post.Route, "POST", type, method, false);
        }
    }
}