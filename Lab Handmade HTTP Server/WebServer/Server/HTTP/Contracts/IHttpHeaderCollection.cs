namespace WebServer.Server.HTTP.Contracts
{
	using System;
	
    public interface IHttpHeaderCollection
    {
        void Add(HttpHeader header);

        bool ContainsKey(string key);

        HttpHeader GetHeader(string key);
    }
}
