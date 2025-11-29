using System.Net;

namespace MiniHttpServer.Framework.Core.Abstracts;

public abstract class Handler
{
    public Handler Successor { get; set; }
    public abstract void HandleRequest(HttpListenerContext context);
}