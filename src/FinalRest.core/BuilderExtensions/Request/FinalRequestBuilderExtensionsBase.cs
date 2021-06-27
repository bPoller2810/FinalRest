using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public static class FinalRequestBuilderExtensionsBase
    {

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

            builder.AsyncResponseBehaviours.AddRange(self.AsyncResponseBehaviours);
            builder.ResponseBehaviours.AddRange(self.ResponseBehaviours);

            return builder;
        }

        #region setter
        public static FinalRestRequestBuilder SetRoute(this FinalRestRequestBuilder self, string route)
        {
            self.Route = route;
            return self;
        }

        public static FinalRestRequestBuilder SetMethod(this FinalRestRequestBuilder self, ERestMethod method)
        {
            self.Method = method;
            return self;
        }

        public static FinalRestRequestBuilder SetRequestBodyType(this FinalRestRequestBuilder self, EBodyType bodyType)
        {
            self.BodyType = bodyType;
            return self;
        }
        #endregion

        #region header
        public static FinalRestRequestBuilder AddHeader(this FinalRestRequestBuilder self, string key, string value)
        {
            self.Headers.Add(key, value);
            return self;
        }
        #endregion

        #region request handler
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
        public static FinalRestRequestBuilder AddAsyncResponseBehaviour<TResult>(this FinalRestRequestBuilder self, HttpStatusCode statusCode, Func<HttpStatusCode, TResult, Task> behaviour)
            where TResult : class
        {
            var convertedFunction = new Func<HttpStatusCode, object, Task>((status, data) => behaviour.Invoke(status, (TResult)data));
            self.AsyncResponseBehaviours.Add((statusCode, convertedFunction));
            return self;
        }
        public static FinalRestRequestBuilder AddResponseBehaviour<TResult>(this FinalRestRequestBuilder self, HttpStatusCode statusCode, Action<HttpStatusCode, TResult> behaviour)
            where TResult : class
        {
            var convertedAction = new Action<HttpStatusCode, object>((statusCode, data) => behaviour.Invoke(statusCode, (TResult)data));
            self.ResponseBehaviours.Add((statusCode, convertedAction));
            return self;
        }
        #endregion


    }

}
