using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public static class FinalRequestBuilderExtensionsBuild
    {

        /// <summary>
        /// This Creates a FinalRestRequest
        /// </summary>
        /// <param name="self">The Builder as extended Method</param>
        /// <returns>A FinalRestRequest that represents the configuration of this Builder</returns>
        public static IRestRequest Build(this FinalRestRequestBuilder self)
        {
            #region validation
            if (string.IsNullOrWhiteSpace(self.Route))
            {
                throw new InvalidOperationException("You must set a Route");
            }
            #endregion

            return new FinalRestRequest
            {
                Method = self.Method != ERestMethod.Default ? self.Method : ERestMethod.GET,
                Encoding = self.Encoding ?? Encoding.UTF8,
                MediaType = self.MediaType ?? "application/json",
                Route = self.Route,
                BodyType = self.BodyType != EBodyType.DEFAULT ? self.BodyType : EBodyType.JSON,
                Headers = GetHeaders(self.Headers),

                PreRequestHandler = GetPreRequestHandlerInstances(self.PreRequestHandler),
                PostRequestHandler = GetPostRequestHandlerInstances(self.PostRequestHandler),

                AsyncResultBehaviours = GetAsyncResultBehaviourDefinitions(self.AsyncResultBehaviours),
                AsyncResponseBehaviours = GetAsyncResponseBehaviourDefinitions(self.AsyncResponseBehaviour),

                ResultBehaviours = GetResultBehaviourDefinitions(self.ResultBehaviours),
                ResponseBehaviours = GetResponseBehaviourDefinitions(self.ResponseBehaviours)
            };
        }



        #region helper
        private static FinalRestHeaderCollection GetHeaders(Dictionary<string, string> headers)
        {
            return headers
                .Select(h => new FinalRestHeader(h.Key, h.Value))
                .ToFinalRequestHeaderCollection();

        }
        private static IPreRequestHandler[] GetPreRequestHandlerInstances(IEnumerable<Type> preRequestHandlers)
        {
            return preRequestHandlers
                .Select(h => GlobalRestRequestHandlerStore.Store.GetHandlerInstance<IPreRequestHandler>(h))
                .ToArray();
        }
        private static IPostRequestHandler[] GetPostRequestHandlerInstances(IEnumerable<Type> postRequestHandlers)
        {
            return postRequestHandlers
                .Select(h => GlobalRestRequestHandlerStore.Store.GetHandlerInstance<IPostRequestHandler>(h))
                .ToArray();
        }

        private static AsyncResultBehaviourDefinition[] GetAsyncResultBehaviourDefinitions(IEnumerable<(HttpStatusCode StatusCode, Func<HttpStatusCode, object, Task> Behaviour)> behaviours)
        {
            return behaviours
                .Select(a => new AsyncResultBehaviourDefinition
                {
                    StatusCode = a.StatusCode,
                    Behaviour = a.Behaviour,
                })
                .ToArray();
        }
        private static AsyncResponseBehaviourDefinition[] GetAsyncResponseBehaviourDefinitions(IEnumerable<(HttpStatusCode StatusCode, Func<HttpStatusCode, Task> Behaviour)> behaviours)
        {
            return behaviours
                .Select(a => new AsyncResponseBehaviourDefinition
                {
                    StatusCode = a.StatusCode,
                    Behaviour = a.Behaviour,
                })
                .ToArray();
        }
        private static ResultBehaviourDefinition[] GetResultBehaviourDefinitions(IEnumerable<(HttpStatusCode StatusCode, Action<HttpStatusCode, object> Behaviour)> behaviour)
        {
            return behaviour
                .Select(r => new ResultBehaviourDefinition
                {
                    StatusCode = r.StatusCode,
                    Behaviour = r.Behaviour,
                })
                .ToArray();
        }
        private static ResponseBehaviourDefinition[] GetResponseBehaviourDefinitions(IEnumerable<(HttpStatusCode StatusCode, Action<HttpStatusCode> Behaviour)> behaviour)
        {
            return behaviour
                .Select(r => new ResponseBehaviourDefinition
                {
                    StatusCode = r.StatusCode,
                    Behaviour = r.Behaviour,
                })
                .ToArray();
        }
        #endregion

    }
}
