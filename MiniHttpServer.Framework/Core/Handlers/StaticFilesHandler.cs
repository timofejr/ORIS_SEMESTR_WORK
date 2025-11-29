using System.Net;
using System.Text;
using MiniHttpServer.Framework.Context;
using MiniHttpServer.Framework.Core.Abstracts;
using MiniHttpServer.Framework.Settings;
using MiniHttpServer.Framework.Utils;

namespace MiniHttpServer.Framework.Core.Handlers;

public class StaticFilesHandler : Handler
{
    public override void HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var isGetMethod = request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase);
        var isStaticFile = request.Url.AbsolutePath.Split('/').Any(x=> x.Contains("."));
        
        if (isGetMethod && isStaticFile)
        {
            string path = request.Url.AbsolutePath.Trim('/');
            
            GlobalContext.Server.SendStaticResponse(context, HttpStatusCode.OK, path);
        }
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}