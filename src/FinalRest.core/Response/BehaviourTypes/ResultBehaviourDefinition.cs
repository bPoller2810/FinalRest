using System;
using System.Net;

namespace FinalRest.core
{
    public sealed class ResultBehaviourDefinition
    {
        public HttpStatusCode StatusCode { get; internal set; }

        public Action<HttpStatusCode, object> Behaviour { get; internal set; }
    }
}
