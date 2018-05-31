namespace WebServer.Server
{
    using Server.Contracts;
    using Server.Routing;
    using Server.Routing.Contracts;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class MyWebServer : IRunnable
    {
        private const string localHostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly IServerRouteConfig serverRouteConfig;

        private readonly TcpListener listener;

        private bool  isRunning;

        public MyWebServer(int port, IAppRouteConfig routeConfig)
        {
            this.port = port;

            this.listener = new TcpListener(IPAddress.Parse(localHostIpAddress),port);

            this.serverRouteConfig = new ServerRouteConfig(routeConfig);
        }
        public void Run()
        {
            this.listener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server running on {localHostIpAddress}:{port}");

            Task.Run(this.ListenLoop).Wait();

        }

        private async Task ListenLoop()
        {
            while (this.isRunning)
            {
                var client = await this.listener.AcceptSocketAsync();
                var connectionHandler = new ConnectionHandler(client, this.serverRouteConfig);
                var connection = connectionHandler.ProcessRequestAsync();
                connection.Wait();
            }
        }
    }
}
