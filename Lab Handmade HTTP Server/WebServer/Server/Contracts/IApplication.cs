namespace WebServer.Server.Contracts
{
	using System;
    using WebServer.Server.Routing.Contracts;

    public interface IApplication
    {
        void Configure(IAppRouteConfig appRouteConfig);
    }
}
