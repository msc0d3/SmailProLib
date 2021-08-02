using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmailproLib
{
    public sealed class SmailProConfig
    {
        /// <summary>
        /// payloads to get data
        /// </summary>
        public struct Payload
        {
            /// <summary>
            /// payload to create new auth api key
            /// </summary>
            /// <param name="config"></param>
            /// <returns></returns>
            public static string PayloadCreateAuthApiKey(SmailProConfig config)
            {
                return $"username={config.username}&domain={config.domain}";
            }
            /// <summary>
            /// payload to create email
            /// </summary>
            /// <param name="config"></param>
            /// <param name="authdata"></param>
            /// <returns></returns>
            public static string PayloadCreateEmail(SmailProConfig config, Smailpro.SmailProAuthData authdata)
            {
                return $"{rapidapiUrl}email/gd/get?key={authdata.AuthApiKey}&rapidapi-key={Smailpro.SmailProAuthData.rapidapi_key}&username={config.username}&domain={config.domain}";
            }
            /// <summary>
            /// payload to check mailbox
            /// </summary>
            /// <param name="authdata"></param>
            /// <returns></returns>
            public static string PayloadCheckEmail(Smailpro.SmailProAuthData authdata)
            {
                return $"{rapidapiUrl}email/gd/check?key={authdata.AuthApiKey}&rapidapi-key={Smailpro.SmailProAuthData.rapidapi_key}&email={authdata.email}&timestamp={authdata.timestamp}";
            }
            /// <summary>
            /// payload to refresh auth token
            /// </summary>
            /// <param name="authdata"></param>
            /// <returns></returns>
            public static string PayloadRefreshAuthToken(Smailpro.SmailProAuthData authdata)
            {
                return $"https://smailpro.com/app/key?email={authdata.email}&timestamp={authdata.timestamp}";
            }
        }
        /// <summary>
        /// mail url
        /// </summary>
        public static string HomeUrl
        {
            get => "https://smailpro.com/advanced";
        }
        /// <summary>
        /// api url to check mail, token... etc
        /// </summary>
        public static string rapidapiUrl
        {
            get => "https://public-sonjj.p.rapidapi.com/";
        }
        /// <summary>
        /// list domains
        /// </summary>
        private static string[] SmailProDomains = new string[] { "cardgener.com", "ugener.com", "ychecker.com", "storegmail.com", "instasmail.com" };
        /// <summary>
        /// username of email
        /// </summary>
        public string username
        {
            get
            {
                return user_;
            }
            set
            {
                if(string.IsNullOrEmpty(username))
                {
                    user_ = randomUserName;
                }
                else { user_ = value; }
            }
        }
        /// <summary>
        /// domain of email
        /// </summary>
        public string domain
        {
            get
            {
                return domain_;
            }
            set
            {
                if (string.IsNullOrEmpty(domain))
                {
                    domain_ = randomDomain;
                }
                else { domain_ = value; }
            }
        }
        private string user_;
        private string domain_;
        /// <summary>
        /// init random username
        /// </summary>
        public static string randomUserName
        {
            get => "random";
        }
        /// <summary>
        /// init random domain
        /// </summary>
        public static string randomDomain
        {
            get => SmailProDomains[new Random().Next(SmailProDomains.Count())];
        }
    }
}
