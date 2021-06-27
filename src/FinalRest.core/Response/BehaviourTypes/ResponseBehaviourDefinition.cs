using System;
using System.Net;

namespace FinalRest.core
{
    public sealed class ResponseBehaviourDefinition
    {
        public HttpStatusCode StatusCode { get; internal set; }

        public Action<HttpStatusCode, object> Behaviour { get; internal set; }
    }
}
