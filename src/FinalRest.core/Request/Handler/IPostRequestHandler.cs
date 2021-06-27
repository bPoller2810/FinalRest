using System.Net;

namespace FinalRest.core
{
    public interface IPostRequestHandler
    {
        void HandlePostRequest<TDataType>(HttpStatusCode statusCode, TDataType data);
    }
}
