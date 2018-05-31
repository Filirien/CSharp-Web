namespace WebServer.Server.Handlers
{
    using System;
    using Server.Common;
    using Server.HTTP;
    using Server.HTTP.Contracts;
    using Server.Handlers.Contracts;

    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> handlingFunc;

        protected RequestHandler(Func<IHttpRequest, IHttpResponse> handlingFunc)
        {
            CoreValidator.ThrowIfNull(handlingFunc, nameof(handlingFunc));
            this.handlingFunc = handlingFunc;
        }
        public IHttpResponse Handle(IHttpContext context)
        {
            var response = this.handlingFunc(context.Request);
            response.Headers.Add(new HttpHeader("Context-Type", "text/html"));

            return response;
        }
        
    }
}
