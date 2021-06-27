using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace FinalRest.core
{
    public sealed class FinalRestClientBuilder<TInvocationKey>
    {
        public string BaseUrl { get; internal set; }
        public Func<IHttpClient> HttpClientFactory { get; internal set; }
        public Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> CustomCertificateValidation { get; set; }
        public Dictionary<TInvocationKey, IRestRequest> Requests { get; } = new Dictionary<TInvocationKey, IRestRequest>();
    }

}
