namespace WebServer.Application.Controllers
{
	using System;
    using System.Net;
    using WebServer.Application.Views;
    using WebServer.Server.HTTP.Contracts;
    using WebServer.Server.HTTP.Response;

    public class HomeController
    {
        //GET /
		public IHttpResponse Index()
        {
            return new ViewResponse(HttpStatusCode.OK, new IndexView());
        }
    }
}
