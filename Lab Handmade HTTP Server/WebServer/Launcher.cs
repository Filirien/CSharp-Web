namespace WebServer
{
    using WebServer.Application;
    using WebServer.Server;
    using WebServer.Server.Contracts;
    using WebServer.Server.Routing;
    public class Launcher : IRunnable
    {

        public static void Main()
        {
            new Launcher().Run();
        }

        public void Run()
        {
            var mainApplication = new MainApplication();
            var appRouteConfig = new AppRouteConfig();
            mainApplication.Configure(appRouteConfig);

            var webServer = new MyWebServer(1337, appRouteConfig);

            webServer.Run();
        }
    }
}
