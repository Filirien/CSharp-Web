namespace WebServer.Server.HTTP.Response
{
	using System;
    using System.Net;

    public class NotFoundResponse : HttpResponse
    {
        public NotFoundResponse()
        {
            this.StatusCode = HttpStatusCode.NotFound;
        }
    }
}
