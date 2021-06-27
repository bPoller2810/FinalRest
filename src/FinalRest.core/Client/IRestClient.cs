using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public interface IRestClient<TInvocationKey>
    {
        string BaseUrl { get; }
        IReadOnlyDictionary<TInvocationKey, IRestRequest> Requests { get; }
        Func<IHttpClient> HttpClientFactory { get; }

        Task<TResult> InvokeAsync<TResult>(TInvocationKey key, object body = null, params string[] urlParameters)
            where TResult : class;
    }

}
