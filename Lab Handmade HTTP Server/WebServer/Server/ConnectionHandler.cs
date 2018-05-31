namespace WebServer.Server
{
    using Server.Common;
    using Server.Handlers;
    using Server.HTTP;
    using Server.HTTP.Contracts;
    using Server.Routing.Contracts;
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IServerRouteConfig serverRouteConfig;

        public ConnectionHandler(Socket client, IServerRouteConfig serverRouteConfig)
        {
            CoreValidator.ThrowIfNull(client,nameof(client));
            CoreValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            this.client = client;
            this.serverRouteConfig = serverRouteConfig;
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest =  await this.ReadRequest();
            var httpContext = new HttpContext(httpRequest);
            var httpResponse = new HttpHandler(this.serverRouteConfig).Handle(httpContext);

            var toBytes = new ArraySegment<byte>(Encoding.UTF8.GetBytes(httpResponse.ToString()));

            await this.client.SendAsync(toBytes, SocketFlags.None);
            Console.WriteLine($"-----REQUEST-----");
            Console.WriteLine(httpRequest);
            Console.WriteLine($"-----RESPONSE-----");
            Console.WriteLine(httpResponse.ToString());

            this.client.Shutdown(SocketShutdown.Both);
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            var request = string.Empty;
            var data = new ArraySegment<byte>(new byte[1024]);

            int numBytesRead;

            while ((numBytesRead = await this.client.ReceiveAsync(data, SocketFlags.None)) >0 )
            {
                request += Encoding.UTF8.GetString(data.Array, 0, numBytesRead);
                if (numBytesRead < 1023) 
                {
                    break;
                }
            }
            return new HttpRequest(request);
        }
    }
}
