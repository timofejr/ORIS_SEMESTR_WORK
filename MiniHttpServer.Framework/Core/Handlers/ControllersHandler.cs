// ControllersHandler.cs
using System.Net;
using System.Reflection;
using MiniHttpServer.Framework.Context;
using MiniHttpServer.Framework.Core.Abstracts;
using MiniHttpServer.Framework.Core.Attributes;
using MiniHttpServer.Framework.Core.HttpResponse;
using MiniHttpServer.Framework.Utils;
using System.Text.RegularExpressions;

namespace MiniHttpServer.Framework.Core.Handlers;

public class ControllersHandler : Handler
{
    public override void HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var endpointPath = request.Url?.AbsolutePath.Trim('/');
        var method = request.HttpMethod;

        bool endpointFound = false;

        foreach (var endpoint in GlobalContext.Endpoints)
        {
            var (routePattern, httpMethod) = endpoint.Key;

            if (httpMethod == method && routePattern.IsMatch(endpointPath))
            {
                var (type, methodInfo, hasRouteParameter) = endpoint.Value;

                object[] methodParams = null;
                if (hasRouteParameter)
                {
                    var match = routePattern.Match(endpointPath);
                    if (!match.Success)
                    {
                        GlobalContext.Server.Send404Response(context, endpointPath);
                        return;
                    }
                    string id = match.Groups[1].Value;
                    methodParams = new object[] { id };
                }

                var controllerInstance = Activator.CreateInstance(type);
                (controllerInstance as BaseController)?.SetContext(context);
                var result = methodInfo.Invoke(controllerInstance, methodParams);

                var resultString = (result as IResponseResult)?.Execute(context);
                var resultStatusCode = (result as IResponseResult).StatusCode;

                if (result is PageResult)
                {
                    GlobalContext.Server.SendPageResponse(context, resultStatusCode, resultString);
                }
                else if (result is JsonResult)
                {
                    GlobalContext.Server.SendJsonResponse(context, resultStatusCode, resultString);
                }

                return;
            }
        }
        
        if (!endpointFound)
        {
            GlobalContext.Server.Send404Response(context, endpointPath);
        }

        if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}