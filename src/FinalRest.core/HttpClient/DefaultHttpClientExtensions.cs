namespace FinalRest.core
{
    public static class DefaultHttpClientExtensions
    {
        public static FinalRestClientBuilder<TInvocationKey> UseHttpClient<TInvocationKey>(this FinalRestClientBuilder<TInvocationKey> self)
        {
            self.HttpClientFactory = () => new DefaultHttpClient();
            return self;
        }
    }

}
