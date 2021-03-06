using System;
using System.Net;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public sealed class AsyncResponseBehaviourDefinition
    {
        public HttpStatusCode StatusCode { get; internal set; }
        public Func<HttpStatusCode, Task> Behaviour { get; internal set; }
    }
}
