using System.Reflection;
using MiniHttpServer.Framework;
using MiniHttpServer.Framework.Utils;
using MiniHttpServer.Framework.Context;
using MiniHttpServer.Framework.Settings;
using MiniHttpServer.Utils;
using MyORMLibrary;

try
{   
    EndpointsRegistry.LoadEndpoints(Assembly.GetExecutingAssembly());
    
    GlobalContext.Server = new HttpServer();
    GlobalContext.Logger = Logger.Instance;
    GlobalContext.SettingsManager = SettingsManager.Instance;
    GlobalContext.Endpoints = EndpointsRegistry.LoadEndpoints(Assembly.GetExecutingAssembly());
    GlobalContext.OrmContext = new ORMContext(Environment.GetEnvironmentVariable("ConnectionString"));

    GlobalContext.Server.Start();
    

    while (true)
    {
        // var input = await Task.Run(Console.ReadLine);
        // if (input.ToLower().Equals("/stop"))
        //     break;
    }
    
    GlobalContext.Server.Stop();
}
catch (Exception ex)
{
    GlobalContext.Logger.LogMessage(ex.Message);
    GlobalContext.Logger.LogMessage(ex.StackTrace);
}   
finally
{
    GlobalContext.Logger.ServerStopped();
}