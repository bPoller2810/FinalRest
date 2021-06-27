using System;
using System.Collections.ObjectModel;

namespace FinalRest.core
{
    public static class FinalClientBuilderExtensionsBuild
    {

        public static IRestClient<TInvocationKey> Build<TInvocationKey>(this FinalRestClientBuilder<TInvocationKey> self)
        {
            #region validation
            if (string.IsNullOrWhiteSpace(self.BaseUrl))
            {
                throw new InvalidOperationException("You must set a BaseUrl");
            }
            if (self.Requests.Count == 0)
            {
                throw new InvalidOperationException("You need at least 1 Request");
            }
            if (self.HttpClientFactory is null)
            {
                throw new InvalidOperationException("You must specify a HttpClient type");
            }
            #endregion

            return new FinalRestClient<TInvocationKey>
            {
                BaseUrl = self.BaseUrl,
                Requests = new ReadOnlyDictionary<TInvocationKey, IRestRequest>(self.Requests),
                HttpClientFactory = self.HttpClientFactory,
                CustomCertificateValidation = self.CustomCertificateValidation,
            };
        }

    }
}
