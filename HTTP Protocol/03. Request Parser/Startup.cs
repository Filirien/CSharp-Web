using System;
using System.Collections.Generic;
using System.Linq;

namespace _03._Request_Parser
{
    public class Startup
    {
        private static string HttpResponseHeader =
            "HTTP/1.1 {0} {2}\nContent-Lenght: {1}\nContent-Type: text/plain\n\n{2}";
        private static Dictionary<int, string> StatusTextByResponseCode = new Dictionary<int, string>() {

            {200,"OK"},
            {404,"Not Found" }
        };

        public static void Main(string[] args)
        {
            var pathsByMethods = new Dictionary<string, HashSet<string>>();

            var input = Console.ReadLine();
            while (input != "END")
            {
                var splitInput = input.Substring(1).Split("/");
                var method = splitInput[1];
                var path = splitInput[0];
                if (!pathsByMethods.ContainsKey(method))
                {
                    pathsByMethods.Add(method, new HashSet<string>() { path });
                }
                else
                {
                    pathsByMethods[method].Add(path);
                }
                input = Console.ReadLine();
            }
            var request = Console.ReadLine();
            var requestSplit = request.Split(' ');
            var requestMethod = requestSplit[0].ToLower();
            var requestPath = requestSplit[1].Substring(1).ToLower();
            var responsePath = pathsByMethods.ContainsKey(requestMethod) ? pathsByMethods[requestMethod].FirstOrDefault(p=>p.ToLower() == requestPath) : string.Empty;

            var response = string.Empty;
            if (string.IsNullOrEmpty(responsePath))
            {
                var responseStatusCode = $"{StatusTextByResponseCode.Keys.FirstOrDefault(k => k == 404)}";

                response = string.Format(
                   HttpResponseHeader,
                   responseStatusCode,
                   StatusTextByResponseCode[404].Length,
                   StatusTextByResponseCode[404]
                   );
            }
            else
            {
                var responseStatusCode = $"{StatusTextByResponseCode.Keys.FirstOrDefault(k => k == 200)}";
                response = string.Format(
                    HttpResponseHeader,
                    responseStatusCode,
                    StatusTextByResponseCode[200].Length,
                    StatusTextByResponseCode[200]
                    );
            }
            Console.WriteLine(response);
        }
    }
}
