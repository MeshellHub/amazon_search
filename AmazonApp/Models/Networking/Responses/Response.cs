using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonApp.Models.Networking.Responses
{
    public class Response
    {
        public bool IsOk
        {
            get
            {
                return string.IsNullOrWhiteSpace(ErrorMessage);
            }
        }

        public string ErrorMessage { get; set; }
    }
}