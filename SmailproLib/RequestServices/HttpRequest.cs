using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace SmailproLib.RequestServices
{
    /// <summary>
    /// http reques class
    /// </summary>
    public class HttpRequest:HttpResponse
    {
        public HttpClient client;
        public HttpRequestConfig config;
        public HttpResponseMessage response;
        public HttpResponse _httpresponse;
        private HttpClientHandler httpClientHandler;
        private bool disposed = false;
        /// <summary>
        /// init new http
        /// </summary>
        /// <param name="httpconfig"></param>
        public HttpRequest(HttpRequestConfig httpconfig)
        {
            httpClientHandler = new HttpClientHandler()
            {
                UseCookies = true,
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            config = httpconfig;
            _httpresponse = new HttpResponse();
            client = new HttpClient(httpClientHandler);
            if (!string.IsNullOrEmpty(config.userAgent))
            {
                client.DefaultRequestHeaders.Add("user-agent", config.userAgent);
            }
            else
            {
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4515.107 Safari/537.36");
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    httpClientHandler.Dispose();
                    client.Dispose();
                }

                disposed = true;
            }
        }
        ~HttpRequest()
        {
            Dispose(false);
        }
    }
}
