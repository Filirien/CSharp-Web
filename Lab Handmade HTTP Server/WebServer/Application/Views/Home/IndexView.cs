namespace WebServer.Application.Views
{
	using System;
    using WebServer.Server.HTTP.Contracts;

    public class IndexView : IView
    {
        public string View() => "<h1>Welcome!</h1>";
    }
}
