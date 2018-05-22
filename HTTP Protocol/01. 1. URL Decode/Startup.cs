using System;
using System.Net;

namespace _01._1._URL_Decode
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            var url = Console.ReadLine();
            var decodedUrl = WebUtility.UrlDecode(url);
            Console.WriteLine(decodedUrl);
        }
    }
}
