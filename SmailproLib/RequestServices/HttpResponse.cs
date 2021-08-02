using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmailproLib.RequestServices
{
    public class HttpResponse
    {
        public enum StatusCode
        {
            Success,
            Error,
            HttpError,
            Unknow
        }
        public string ResponseString;
        public StatusCode _statusCode;
        public void Clean()
        {
            ResponseString = null;
        }
    }
}
