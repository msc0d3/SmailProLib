using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmailproLib.RequestServices;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace SmailproLib
{
    public class Smailpro
    {
        /// <summary>
        /// httpclient
        /// </summary>
        private HttpRequest httpRequest;
        private bool disposed = false;
        /// <summary>
        /// config
        /// </summary>
        public SmailProConfig Config;
        /// <summary>
        /// auth data ( token, email, timestamp ... etc )
        /// </summary>
        public SmailProAuthData Auth;
        /// <summary>
        /// init data
        /// </summary>
        /// <param name="smailProConfig"></param>
        public Smailpro(SmailProConfig smailProConfig)
        {
            HttpRequestConfig _config = new HttpRequestConfig();
            httpRequest = new HttpRequest(_config);
            Config = smailProConfig;
            Auth = new SmailProAuthData();
        }
        /// <summary>
        /// create new session
        /// </summary>
        /// <returns></returns>
        public Task<bool> CreateTask()
        {
            bool isSuccess = false;
            var HttpTask = httpRequest.client.GetAsync(SmailProConfig.HomeUrl);
            HttpTask.Wait();
            string response = string.Empty;
            if(HttpTask.Result.Headers.Contains("set-cookie"))
            {
                Auth.Cookie = HttpTask.Result.Headers.GetValues("set-cookie").Last();
                Auth.XSRF_Token = HttpTask.Result.Headers.GetValues("set-cookie").First();
                httpRequest.client.DefaultRequestHeaders.Add("cookie", Auth.BuildCookieRequestHeader);
                httpRequest.client.DefaultRequestHeaders.Add("x-xsrf-token", Auth.XSRF_Token);
                HttpTask = httpRequest.client.GetAsync($"https://smailpro.com/app/key?{SmailProConfig.Payload.PayloadCreateAuthApiKey(Config)}");
                HttpTask.Wait();
                response = HttpTask.Result.Content.ReadAsStringAsync().Result;
                if(response.Contains("msg\":\"OK\""))
                {
                    Auth.AuthApiKey = Regex.Match(response, "items\":\"(.*?)\"").Groups[1].Value;
                    HttpTask = httpRequest.client.GetAsync(SmailProConfig.Payload.PayloadCreateEmail(Config, Auth));
                    HttpTask.Wait();
                    response = HttpTask.Result.Content.ReadAsStringAsync().Result;
                    Auth.email = Regex.Match(response, "email\":\"(.*?)\"").Groups[1].Value;
                    Auth.timestamp = Convert.ToInt32(Regex.Match(response, "timestamp\":(\\d+)").Groups[1].Value);
                    isSuccess = true;
                } 
                else
                {
                    isSuccess = false;
                }
            }
            else
            {
                isSuccess = false;
            }
            response = string.Empty;
            HttpTask.Dispose();
            return Task.FromResult(isSuccess);
        }
        /// <summary>
        /// check email
        /// </summary>
        /// <returns></returns>
        public Task<SmailProResponse> GetResult()
        {
            SmailProResponse response = new SmailProResponse();
            var httpTask = httpRequest.client.GetAsync(SmailProConfig.Payload.PayloadRefreshAuthToken(Auth));
            httpTask.Wait();
            response.RawText = httpTask.Result.Content.ReadAsStringAsync().Result;
            if (response.RawText.Contains("msg\":\"OK\""))
            {
                Auth.AuthApiKey = Regex.Match(response.RawText, "items\":\"(.*?)\"").Groups[1].Value;
                httpTask = httpRequest.client.GetAsync(SmailProConfig.Payload.PayloadCheckEmail(Auth));
                httpTask.Wait();
                response.RawText = httpTask.Result.Content.ReadAsStringAsync().Result;
                if (response.RawText.Contains("msg\":\"OK\""))
                {
                    response.Status = SmailProResponse.StatusCode.Success;
                }
                else
                {
                    response.Status = SmailProResponse.StatusCode.Error;
                }
            }
            else
            {
                response.Status = SmailProResponse.StatusCode.Error;
            }
            httpTask.Dispose();
            return Task.FromResult(response);
        }
        /// <summary>
        /// auth data
        /// </summary>
        public struct SmailProAuthData
        {
            /// <summary>
            /// rapidapi-key ( you can change it )
            /// </summary>
            public static string rapidapi_key
            {
                get => "f871a22852mshc3ccc49e34af1e8p126682jsn734696f1f081";
            }
            /// <summary>
            /// xsrf-token header value
            /// </summary>
            private string xsrftoken;
            /// <summary>
            /// cookie header value
            /// </summary>
            private string cookie;
            /// <summary>
            /// timestamp header value to check email is expired
            /// </summary>
            public int timestamp
            {
                get;
                set;
            }
            /// <summary>
            /// email
            /// </summary>
            public string email
            {
                get;
                set;
            }
            /// <summary>
            /// api-key to access mailbox
            /// </summary>
            public string AuthApiKey
            {
                get; set;
            }
            /// <summary>
            /// XSRF-Token header value
            /// </summary>
            public string XSRF_Token {
                get
                {
                    return xsrftoken;
                }
                set
                {
                    xsrftoken = Regex.Unescape(Regex.Match(value, "XSRF-TOKEN=(.*?);").Groups[1].Value);
                }
            }
            /// <summary>
            /// cookie
            /// </summary>
            public string Cookie
            {
                get
                {
                    return cookie;
                }
                set
                {
                    cookie = Regex.Unescape(Regex.Match(value, "sonjj_session=(.*?);").Groups[1].Value);
                }
            }
            /// <summary>
            /// main cookie to add to header
            /// </summary>
            public string BuildCookieRequestHeader
            {
                get => $"XSRF-TOKEN={XSRF_Token}; sonjj_session={Cookie};";
            }
        }
        /// <summary>
        /// clean data and finish task
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// clean data and finish task
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    httpRequest.Dispose();
                }
                disposed = true;
            }
        }
        ~Smailpro()
        {
            Dispose(false);
        }
        
    }
}
