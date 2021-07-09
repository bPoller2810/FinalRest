using FinalRest.core;
using System.Diagnostics;
using System.Net;

namespace FinalRest.sample.console
{
    public class CustomJwtAuthHandler : IPreRequestHandler, IPostRequestHandler
    {
        public void HandlePostRequest<TDataType>(HttpStatusCode statusCode, TDataType data)
        {
            Debug.WriteLine($"Post Handling {statusCode} for {data?.GetType().Name}");
        }

        public void HandlePreRequest(FinalRestHeaderCollection headers)
        {
            headers.Add("JWT", "some Token");
        }
    }
}
