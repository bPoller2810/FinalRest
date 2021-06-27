using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FinalRest.core
{
    public static class FinalClientBuilderExtensionsBase
    {

        public static FinalRestClientBuilder<TInvocationKey> SetBaseUrl<TInvocationKey>(this FinalRestClientBuilder<TInvocationKey> self, string baseUrl)
        {
            self.BaseUrl = baseUrl;
            return self;
        }

        public static FinalRestClientBuilder<TInvocationKey> AddRequest<TInvocationKey>(this FinalRestClientBuilder<TInvocationKey> self, IRestRequest request, TInvocationKey key)
        {
            self.Requests.Add(key, request);
            return self;
        }

        public static FinalRestClientBuilder<TInvocationKey> UseCustomCertificateValidation<TInvocationKey>(this FinalRestClientBuilder<TInvocationKey> self, Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> validator)
        {
            self.CustomCertificateValidation = validator;
            return self;
        }


    }
}
