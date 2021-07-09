using BasicMvvm.Models.RandomUserApi;
using FinalRest.core;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FinalRest.sample.console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            #region requests
            var baseRequest = new FinalRestRequestBuilder()
                .AddAsyncResponseBehaviour(HttpStatusCode.OK, (status) => Task.Factory.StartNew(() => Debug.WriteLine(status.ToString())))
                .AddResponseBehaviour(HttpStatusCode.OK, (status)=> Debug.WriteLine($"Sync: {status}"))
                .AddAsyncResultBehaviour<ApiResult>(HttpStatusCode.Unauthorized, (status, data) => Nav.HandleAuthFailAsync($"damn {status} => {data.Info.Version}"))
                .AddResultBehaviour<ApiResult>(HttpStatusCode.InternalServerError, (status, data) => Dialog.ShowErrorDialog(data.Info.Seed))
                .AddResultBehaviour<ApiResult>(HttpStatusCode.OK, (status, data) => Dialog.ShowErrorDialog(data.Info.Seed));

            var randomUsers = baseRequest
                .Copy()
                .SetRoute("api/")
                .SetMethod(ERestMethod.GET)
                .AddPreRequestHandler<CustomJwtAuthHandler>()
                .AddPostRequestHandler<CustomJwtAuthHandler>()
                .AddHeader("Header_1", "MyValue")
                .Build();
            #endregion

            #region client
            var client = new FinalRestClientBuilder<ERequests>()
                .UseHttpClient()
                .SetBaseUrl("https://randomuser.me/")
                .UseCustomCertificateValidation(ValidateCert)
                .AddRequest(randomUsers, ERequests.RandomUsers)
                .Build();
            #endregion

            var users = await client.InvokeAsync<ApiResult>(ERequests.RandomUsers, null, "inc=id,gender,name,email", "results=10");

        }

        private static bool ValidateCert(HttpRequestMessage msg, X509Certificate2 cert, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
