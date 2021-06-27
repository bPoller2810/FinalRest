using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public interface IHttpClient
    {

        void Initialize(string baseUrl, Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> certificateValidator);

        Task<IRestResponse> InvokeAsync<TResult>(IRestRequest request, object body = null, params string[] urlParameters)
            where TResult : class;


    }

}
