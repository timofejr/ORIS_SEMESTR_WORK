using System.Reflection;
using System.Text.RegularExpressions;
using MiniHttpServer.Framework.Settings;
using MiniHttpServer.Utils;
using MyORMLibrary;

namespace MiniHttpServer.Framework.Context;

public class GlobalContext
{
    public static HttpServer Server { get; set; }
    public static Logger Logger { get; set; }
    public static SettingsManager SettingsManager { get; set; }
    public static Dictionary<(Regex RoutePattern, string HttpMethod), (Type Type, MethodInfo Method, bool HasRouteParameter)> Endpoints { get; set; }
    public static ORMContext OrmContext { get; set; } 
}