using System.Net;

namespace FinalRest.core
{
    public interface IPostRequestHandler
    {

        /// <summary>
        /// A callback that acts after the request is made
        /// </summary>
        /// <typeparam name="TDataType">The type of data that the request is returning</typeparam>
        /// <param name="statusCode">The StatusCode of this request</param>
        /// <param name="data">The data that the request is returning</param>
        void HandlePostRequest<TDataType>(HttpStatusCode statusCode, TDataType data);
    }
}
