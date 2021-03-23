using System;
using System.Collections.Generic;
using Leaf.xNet;

namespace FreelanceParser
{
    public class DownloadData
    {
        private HttpRequest _request;
        
        public void Auth(string url)
        {
            //FIXME: we have to moving these fields
            string login = "CheekyDev";
            string password = "m85xt2";

            _request = new HttpRequest(url)
            {
                AcceptEncoding = "gzip, deflate, br",
                UserAgent = Http.ChromeUserAgent(),
                KeepAlive = true,
            };
            
            _request.AddHeader(HttpHeader.Accept, "*/*");
            _request.AddHeader(HttpHeader.AcceptLanguage, "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            _request.AddHeader(HttpHeader.ContentType, "application/x-www-form-urlencoded");
            _request.AddHeader("X-Requested-With", "XMLHttpRequest"); ;

            RequestParams reqParams = new RequestParams
            {
                ["login"] = login,
                ["password"] = password,
                ["store_login"] = "1"
            };

            var response = _request.Post("account/login/", reqParams);
            response.None();
        }


        public string GetPage(string url)
        {
            return _request.Get(url).ToString();
        }


        /// <summary>
        /// Iterates over each page
        /// </summary>
        /// <param name="url"></param>
        /// <param name="numberOfLastPage"></param>
        /// <returns>every page from 1st to last</returns>
        public IEnumerable<string> GetNextPage(string url, int numberOfLastPage)
        {
            for (int numCurrentPage = 1; numCurrentPage <= numberOfLastPage; numCurrentPage++)
            {
                RequestParams reqParams = new RequestParams
                {
                    ["page"] = numCurrentPage
                };

                yield return _request.Get(url, reqParams).ToString();
            }
        }

        public static HttpResponse GetPageWithoutAuthRequest(string url)
        {
            HttpRequest request = new HttpRequest(url)
            {
                AcceptEncoding = "gzip, deflate, br",
                AllowAutoRedirect = true,
                UserAgent = Http.ChromeUserAgent(),
                KeepAlive = false,
            };
            request.AddHeader(HttpHeader.Accept, "text/html");
            request.AddHeader(HttpHeader.AcceptLanguage," en,ru-RU;q=0.9,ru;q=0.8,en-US;q=0.7");

            var response = request.Get("/");
            PrintResponseInfo(response);
            return response;
        }

        private static void PrintResponseInfo(HttpResponse response)
        {
            Console.WriteLine($"Address: {response.Address}");
            Console.WriteLine($"CharacterSet: {response.CharacterSet}");
            Console.WriteLine($"Method: {response.Method}");
            Console.WriteLine($"ContentLength: {response.ContentLength}");
            Console.WriteLine($"ContentType: {response.ContentType}");
            Console.WriteLine($"HasError: {response.HasError}");
            Console.WriteLine($"HasRedirect: {response.HasRedirect}");
            Console.WriteLine($"ProtocolVersion: {response.ProtocolVersion}");
            Console.WriteLine($"ReconnectCount: {response.ReconnectCount}");
            Console.WriteLine($"RedirectAddress: {response.RedirectAddress}");
            Console.WriteLine($"StatusCode: {response.StatusCode}");
        }

        public void Close()
        {
            _request.Close();
        }

    }
}
