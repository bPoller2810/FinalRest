using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public static class FinalRequestBuilderExtensionsBase
    {

        /// <summary>
        /// Copies the current Request Builder to enable the possibility for creating base builders
        /// </summary>
        /// <param name="self">The Builder as extended Method</param>
        /// <returns>The Builder for chaining methods</returns>
        public static FinalRestRequestBuilder Copy(this FinalRestRequestBuilder self)
        {
            var builder = new FinalRestRequestBuilder
            {
                Method = self.Method,
                Route = self.Route,
                BodyType = self.BodyType,
            };
            builder.PreRequestHandler.AddRange(self.PreRequestHandler);
            builder.PostRequestHandler.AddRange(self.PostRequestHandler);

            builder.AsyncResultBehaviours.AddRange(self.AsyncResultBehaviours);
            builder.AsyncResponseBehaviour.AddRange(self.AsyncResponseBehaviour);
            builder.ResultBehaviours.AddRange(self.ResultBehaviours);
            builder.ResponseBehaviours.AddRange(self.ResponseBehaviours);

            return builder;
        }

        #region setter
        /// <summary>
        /// Sets the route to the called endpoint
        /// </summary>
        /// <param name="self">The Builder as extended Method</param>
        /// <param name="route">The Route to the called Endpoint</param>
        /// <returns>The Builder for chaining methods</returns>
        public static FinalRestRequestBuilder SetRoute(this FinalRestRequestBuilder self, string route)
        {
            self.Route = route;
            return self;
        }
        /// <summary>
        /// Sets the Rest Method used for this request
        /// </summary>
        /// <param name="self">The Builder as extended Method</param>
        /// <param name="method">The Rest Method used for this Request</param>
        /// <returns>The Builder for chaining methods</returns>
        public static FinalRestRequestBuilder SetMethod(this FinalRestRequestBuilder self, ERestMethod method, Encoding encoding = null, string mediaType = null)
        {
            self.Method = method;
            self.Encoding = encoding;
            self.MediaType = mediaType;
            return self;
        }

        /// <summary>
        /// Sets the used Content-Type of a post request
        /// </summary>
        /// <param name="self">The Builder as extended Method</param>
        /// <param name="bodyType">The used Content-Type</param>
        /// <returns>The Builder for chaining methods</returns>
        public static FinalRestRequestBuilder SetRequestBodyType(this FinalRestRequestBuilder self, EBodyType bodyType)
        {
            self.BodyType = bodyType;
            return self;
        }
        #endregion

        #region header
        /// <summary>
        /// Adds a custom header to this request
        /// </summary>
        /// <param name="self">The Builder as extended Method</param>
        /// <param name="key">The header name</param>
        /// <param name="value">The header value</param>
        /// <returns>The Builder for chaining methods</returns>
        public static FinalRestRequestBuilder AddHeader(this FinalRestRequestBuilder self, string key, string value)
        {
            self.Headers.Add(key, value);
            return self;
        }
        #endregion

        #region request handler
        /// <summary>
        /// Adds a handler that will be invoked before the request is made
        /// </summary>
        /// <typeparam name="TPreRequestHandler">A Type that implements IPreRequestHandler</typeparam>
        /// <param name="self">The Builder as extended Method</param>
        /// <returns>The Builder for chaining methods</returns>
        public static FinalRestRequestBuilder AddPreRequestHandler<TPreRequestHandler>(this FinalRestRequestBuilder self)
            where TPreRequestHandler : IPreRequestHandler, new()
        {
            if (self.PreRequestHandler.Contains(typeof(TPreRequestHandler)))
            {
                throw new InvalidOperationException($"PreRequestHandler of Type {typeof(TPreRequestHandler).Name} is already registered");
            }
            self.PreRequestHandler.Add(typeof(TPreRequestHandler));
            return self;
        }
        /// <summary>
        /// Adds a handler that will be invoked after the request is made
        /// </summary>
        /// <typeparam name="TPostRequestHandler">A Type that implements IPostRequestHandler</typeparam>
        /// <param name="self">The Builder as extended Method</param>
        /// <returns>The Builder for chaining methods</returns>
        public static FinalRestRequestBuilder AddPostRequestHandler<TPostRequestHandler>(this FinalRestRequestBuilder self)
            where TPostRequestHandler : IPostRequestHandler, new()
        {
            if (self.PostRequestHandler.Contains(typeof(TPostRequestHandler)))
            {
                throw new InvalidOperationException($"PostRequestHandler of Type {typeof(TPostRequestHandler).Name} is already registered");
            }
            self.PostRequestHandler.Add(typeof(TPostRequestHandler));
            return self;
        }
        #endregion

        #region response
        /// <summary>
        /// Adds a async behaviour that will be invoked on a certain StatusCode
        /// </summary>
        /// <typeparam name="TResult">The type of the result of this request</typeparam>
        /// <param name="self">The Builder as extended Method</param>
        /// <param name="statusCode">The StatusCode on wich the behaviour should be invoked</param>
        /// <param name="behaviour">The Behaviour Task that will be invoked</param>
        /// <returns>The Builder for chaining methods</returns>
        public static FinalRestRequestBuilder AddAsyncResultBehaviour<TResult>(this FinalRestRequestBuilder self, HttpStatusCode statusCode, Func<HttpStatusCode, TResult, Task> behaviour)
            where TResult : class
        {
            var convertedFunction = new Func<HttpStatusCode, object, Task>((status, data) => behaviour.Invoke(status, (TResult)data));
            self.AsyncResultBehaviours.Add((statusCode, convertedFunction));
            return self;
        }
        /// <summary>
        /// Adds a async behaviour that will be invoked on a certain StatusCode
        /// </summary>
        /// <param name="self">The Builder as extended Method</param>
        /// <param name="statusCode">The StatusCode on wich the behaviour should be invoked</param>
        /// <param name="behaviour">The Behaviour Task that will be invoked</param>
        /// <returns>The Builder for chaining methods</returns>
        public static FinalRestRequestBuilder AddAsyncResponseBehaviour(this FinalRestRequestBuilder self, HttpStatusCode statusCode, Func<HttpStatusCode, Task> behaviour)
        {
            self.AsyncResponseBehaviour.Add((statusCode, behaviour));
            return self;
        }
        /// <summary>
        /// Adds a behaviour that will be invoked on a certain StatusCode
        /// </summary>
        /// <typeparam name="TResult">The type of the result of this request</typeparam>
        /// <param name="self">The Builder as extended Method</param>
        /// <param name="statusCode">The StatusCode on wich the behaviour should be invoked</param>
        /// <param name="behaviour">The Behaviour Method that will be invoked</param>
        /// <returns>The Builder for chaining methods</returns>
        public static FinalRestRequestBuilder AddResultBehaviour<TResult>(this FinalRestRequestBuilder self, HttpStatusCode statusCode, Action<HttpStatusCode, TResult> behaviour)
            where TResult : class
        {
            var convertedAction = new Action<HttpStatusCode, object>((statusCode, data) => behaviour.Invoke(statusCode, (TResult)data));
            self.ResultBehaviours.Add((statusCode, convertedAction));
            return self;
        }
        /// <summary>
        /// Adds a behaviour that will be invoked on a certain StatusCode
        /// </summary>
        /// <param name="self">The Builder as extended Method</param>
        /// <param name="statusCode">The StatusCode on wich the behaviour should be invoked</param>
        /// <param name="behaviour">The Behaviour Method that will be invoked</param>
        /// <returns>The Builder for chaining methods</returns>
        public static FinalRestRequestBuilder AddResponseBehaviour(this FinalRestRequestBuilder self, HttpStatusCode statusCode, Action<HttpStatusCode> behaviour)
        {
            self.ResponseBehaviours.Add((statusCode, behaviour));
            return self;
        }
        #endregion


    }

}
