using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public sealed class FinalRestRequestBuilder
    {

        internal ERestMethod Method { get; set; }
        internal string Route { get; set; }
        internal EBodyType BodyType { get; set; }

        internal Dictionary<string, string> Headers { get; } = new();

        internal List<Type> PreRequestHandler { get; } = new();
        internal List<Type> PostRequestHandler { get; } = new();

        internal List<(HttpStatusCode StatusCode, Func<HttpStatusCode, object, Task> Behaviour)> AsyncResponseBehaviours { get; } = new();
        internal List<(HttpStatusCode StatusCode, Action<HttpStatusCode, object> Behaviour)> ResponseBehaviours { get; } = new();

    }
}
