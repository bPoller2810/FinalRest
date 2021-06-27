using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public class FinalRestClient<TInvocationKey> : IRestClient<TInvocationKey>
    {

        #region IRestClient<TRequestKey>
        public string BaseUrl { get; internal set; }
        public IReadOnlyDictionary<TInvocationKey, IRestRequest> Requests { get; internal set; }

        internal Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> CustomCertificateValidation { get; set; }
        public Func<IHttpClient> HttpClientFactory { get; internal set; }

        public async Task<TResult> InvokeAsync<TResult>(TInvocationKey key, object body = null, params string[] urlParameters)
            where TResult : class
        {
            var request = GetValidatedRequestByKey(key, body);

            HandlePreRequests(request.PreRequestHandler, request);
            var response = await ExecuteRequestAsync<TResult>(request, body, urlParameters);
            HandlePostRequests<TResult>(request.PostRequestHandler, response);

            HandleSyncBehaviours<TResult>(request.ResponeBehaviours, response);
            await HandleAsyncBehaviours<TResult>(request.AsyncResponseBehaviours, response);

            return response.Data as TResult;
        }
        #endregion

        #region handling methods
        private IRestRequest GetValidatedRequestByKey(TInvocationKey key, object body)
        {
            var request = Requests[key].Copy();//throws if not found
            if (body is not null && request.BodyType == EBodyType.DEFAULT)
            {
                throw new InvalidOperationException("If you supply a body you must set the request BodyType");
            }
            return request;
        }

        private void HandlePreRequests(IPreRequestHandler[] handlers, IRestRequest request)
        {
            foreach (var preRequestHandler in handlers)
            {
                preRequestHandler.HandlePreRequest(request.Headers);
            }
        }
        private void HandlePostRequests<TResult>(IPostRequestHandler[] handlers, IRestResponse response)
            where TResult : class
        {
            foreach (var postRequestHandler in handlers)
            {
                postRequestHandler.HandlePostRequest(response.StatusCode, response.Data as TResult);
            }
        }
        private void HandleSyncBehaviours<TResult>(ResponseBehaviourDefinition[] behaviours, IRestResponse response)
            where TResult : class
        {
            foreach (var syncBehaviour in behaviours.Where(b => b.StatusCode == response.StatusCode))
            {
                syncBehaviour.Behaviour(response.StatusCode, response.Data as TResult);
            }
        }
        private async Task HandleAsyncBehaviours<TResult>(AsyncResponseBehaviourDefinition[] behaviours, IRestResponse response)
            where TResult : class
        {
            foreach (var syncBehaviour in behaviours.Where(b => b.StatusCode == response.StatusCode))
            {
                await syncBehaviour.Behaviour(response.StatusCode, response.Data as TResult);
            }
        }
        #endregion

        #region private execution methods
        private Task<IRestResponse> ExecuteRequestAsync<TResult>(IRestRequest request, object body, params string[] urlParameters)
            where TResult : class
        {
            var httpClient = HttpClientFactory.Invoke();
            httpClient.Initialize(BaseUrl, CustomCertificateValidation);

            return httpClient.InvokeAsync<TResult>(request, body, urlParameters);
        }
        #endregion

    }
}
