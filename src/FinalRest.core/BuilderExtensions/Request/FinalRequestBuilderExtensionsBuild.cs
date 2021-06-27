﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public static class FinalRequestBuilderExtensionsBuild
    {

        public static IRestRequest Build(this FinalRestRequestBuilder self)
        {
            #region validation
            if (self.Method == ERestMethod.Default)
            {
                throw new InvalidOperationException("You must set a Method");
            }
            if (string.IsNullOrWhiteSpace(self.Route))
            {
                throw new InvalidOperationException("You must set a Route");
            }
            #endregion

            return new FinalRestRequest
            {
                Method = self.Method,
                Route = self.Route,
                BodyType = self.BodyType,
                Headers = GetHeaders(self.Headers),
                PreRequestHandler = GetPreRequestHandlerInstances(self.PreRequestHandler),
                PostRequestHandler = GetPostRequestHandlerInstances(self.PostRequestHandler),
                AsyncResponseBehaviours = GetAsyncResponseBehaviourDefinitions(self.AsyncResponseBehaviours),
                ResponeBehaviours = GetResponseBehaviourDefinitions(self.ResponseBehaviours),
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

        private static AsyncResponseBehaviourDefinition[] GetAsyncResponseBehaviourDefinitions(IEnumerable<(HttpStatusCode StatusCode, Func<HttpStatusCode, object, Task> Behaviour)> behaviours)
        {
            return behaviours
                .Select(a => new AsyncResponseBehaviourDefinition
                {
                    StatusCode = a.StatusCode,
                    Behaviour = a.Behaviour,
                })
                .ToArray();
        }
        private static ResponseBehaviourDefinition[] GetResponseBehaviourDefinitions(IEnumerable<(HttpStatusCode StatusCode, Action<HttpStatusCode, object> Behaviour)> behaviour)
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
