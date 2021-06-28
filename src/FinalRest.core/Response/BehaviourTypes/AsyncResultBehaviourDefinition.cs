using System;
using System.Net;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public sealed class AsyncResultBehaviourDefinition
    {
        public HttpStatusCode StatusCode { get; internal set; }
        public Func<HttpStatusCode, object, Task> Behaviour { get; internal set; }
    }
}
