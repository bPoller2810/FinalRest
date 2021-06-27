using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FinalRest.core
{
    public class FinalRestResponse : IRestResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public object Data { get; set; }

        public string Reason { get; set; }

        public Exception Exception { get; set; }
    }
}
