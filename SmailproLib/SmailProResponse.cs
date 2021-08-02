using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmailproLib
{
    public class SmailProResponse
    {
        /// <summary>
        /// list of status task
        /// </summary>
        public enum StatusCode
        {
            Processing,
            Success,
            Error,
            CaptchaRequire
        }
        /// <summary>
        /// processing - task is not ready yet
        /// ready - task complete, solution object can be found in solution property
        /// </summary>
        public StatusCode Status { get; set; }
        /// <summary>
        /// response text , raw text from smailPro
        /// </summary>
        public string RawText { get; set; }
    }
}
