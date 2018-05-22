using System;
using System.Text.RegularExpressions;

namespace _02._Validate_URL
{
    public class Startup
    {
        private static string ValidUrlSummaryTempalte = "Protocol: {0}\nHost: {1}\nPort: {2}\nPath: {3}\nQuery: {4}\nFragments: {5}";

        public static void Main(string[] args)
        {
            var url = Console.ReadLine();
            var pattern = @"^((http|https):\/(\/[0-9a-zA-Z\-\.]+)(?:\:(\d+))?)(\/([0-9a-zA-Z\-\.\/]+)*)*(\?[0-9a-zA-Z\-\.\=\+\&_]+)?(\#[0-9a-zA-Z\-\.]+)?$";

            var urlMatcher = new Regex(pattern);
            var matchedUrl = urlMatcher.Match(url);
            if (matchedUrl.Success)
            {
                var matches = matchedUrl.Groups;
                var protocolMatch = matches[2].ToString();
                var port = matches[4].ToString();

                if (protocolMatch.ToString() == "https")
                {
                    if (string.IsNullOrEmpty(port) 
                        || port.ToString() != "443")
                    {
                        Console.WriteLine("Invalid URL!");
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(port)
                        && port.ToString() != "80")
                    {
                        Console.WriteLine("Invalid URL!");
                        return;
                    }
                    else if(string.IsNullOrEmpty(port))
                    {
                        port = "80";
                    }
                }
                var host = matches[3].ToString();
                var path = matches[5].ToString();
                var query = matches[7].ToString();
                var fragment = matches[8].ToString();
                var validUrl = string.Format(ValidUrlSummaryTempalte
                    , protocolMatch,
                    host,
                    port,
                    path,
                    query,
                    fragment);
                Console.WriteLine(validUrl);
            }
            else
            {
                Console.WriteLine("Invalid url!");
            }

        }
    }
}
