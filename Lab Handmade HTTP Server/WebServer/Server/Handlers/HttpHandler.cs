namespace WebServer.Server.Handlers
{
    using Server.Common;
    using Server.Handlers.Contracts;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Server.Routing.Contracts;
    using System.Text.RegularExpressions;

    public class HttpHandler :IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig routeConfig)
        {
            CoreValidator.ThrowIfNull(routeConfig, nameof(routeConfig));

            this.serverRouteConfig = routeConfig;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            var requestMethod = context.Request.Method;
            var requestPath = context.Request.Path;
            var registeredRoutes = this.serverRouteConfig.Routes[requestMethod];
            foreach (var registeredRoute in registeredRoutes)
            {
                var routePattern = registeredRoute.Key;
                var routingContext = registeredRoute.Value;
                var routeRegex = new Regex(routePattern);
                Match match = routeRegex.Match(requestPath);

                if (!match.Success)
                {
                    continue;
                }
                var parameters = routingContext.Parameters;
                foreach (var parameter in parameters)
                {
                    var parameterValue = match.Groups[parameter].Value;
                    context.Request.AddUrlParameter(parameter, parameterValue);
                }
                return routingContext.Handler.Handle(context);
            }
            return new NotFoundResponse();
        }
    }
}
