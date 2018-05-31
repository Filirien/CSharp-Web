namespace WebServer.Server.Routing
{
    using Enums;
    using Handlers;
    using Routing.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AppRouteConfig : IAppRouteConfig
    {
        private readonly Dictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> routes;
        public IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> Routes => this.routes;

        public AppRouteConfig()
        {
            this.routes = new Dictionary<HttpRequestMethod, IDictionary<string, RequestHandler>>();
            var availableMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (var method in availableMethods)
            {
                this.routes[method] = new Dictionary<string, RequestHandler>();
            }
        }
        public void AddRoute(string route, RequestHandler handler)
        {
            var hadnlerName = handler.GetType().ToString().ToLower();
            if (hadnlerName.Contains("get"))
            {
                this.routes[HttpRequestMethod.Get].Add(route,handler);
            }
            else if (hadnlerName.Contains("post"))
            {
                this.routes[HttpRequestMethod.Post].Add(route,handler);
            }
            else
            {
                throw new InvalidOperationException("Invalid handler.");
            }
        }
    }
}
