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
            #endregion

            return new FinalRestClient<TInvocationKey>
            {
                BaseUrl = self.BaseUrl,
                Requests = new ReadOnlyDictionary<TInvocationKey, IRestRequest>(self.Requests),
                HttpClientFactory = self.HttpClientFactory is not null ? self.HttpClientFactory : () => new DefaultHttpClient(),
                CustomCertificateValidation = self.CustomCertificateValidation,
            };
        }

    }
}
