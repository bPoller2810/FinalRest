using BasicMvvm.Models.RandomUserApi;
using FinalRest.core;
using System;
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
               .AddAsyncResponseBehaviour<ApiResult>(HttpStatusCode.Unauthorized, (status, data) => Nav.HandleAuthFailAsync($"damn {status} => {data.Info.Version}"))
               .AddResponseBehaviour<ApiResult>(HttpStatusCode.InternalServerError, (status, data) => Dialog.ShowErrorDialog(data.Info.Seed))
               .AddResponseBehaviour<ApiResult>(HttpStatusCode.OK, (status, data) => Dialog.ShowErrorDialog(data.Info.Seed));

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

            //TODO: non generic behaviours (dont respond with the object)


        }

        private static bool ValidateCert(HttpRequestMessage msg, X509Certificate2 cert, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }

    public record AuthDto(string Username, string Password);

    partial class MyCustomClient
    {
        public bool IsCool { get; set; }
    }


    public class Nav
    {
        public static Task HandleAuthFailAsync(string response)
        {
            Debug.WriteLine($"Auth failes with: {response}");
            return Task.CompletedTask;
        }
    }
    public class Dialog
    {
        public static void ShowErrorDialog(string error)
        {
            Console.WriteLine(error);
        }
    }

    public class CustomJwtAuthHandler : IPreRequestHandler, IPostRequestHandler
    {
        public void HandlePostRequest<TDataType>(HttpStatusCode statusCode, TDataType data)
        {
            Debug.WriteLine($"Post Handling {statusCode} for {data?.GetType().Name}");
        }

        public void HandlePreRequest(FinalRestHeaderCollection headers)
        {
            headers.Add("JWT", "some Token");
        }
    }

    public class SessionResultDto
    {
        public string Token { get; set; }
    }
    public class DataDto
    {
        public string Data { get; set; }
    }


    public enum ERequests
    {
        Auth, Other, DataGet, RandomUsers,
    }
}
