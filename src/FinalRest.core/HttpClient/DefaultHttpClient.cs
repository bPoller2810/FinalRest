using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public class DefaultHttpClient : IHttpClient
    {
        #region private member
        private HttpClient _httpClient;
        #endregion

        #region IHttpClient
        public void Initialize(string baseUrl, Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> certificateValidator)
        {
            #region handler
            var httpHandler = new HttpClientHandler();
            if (certificateValidator is not null)
            {
                httpHandler.ServerCertificateCustomValidationCallback = certificateValidator;
            }
            #endregion
            #region client
            _httpClient = new HttpClient(httpHandler)
            {
                BaseAddress = new Uri(baseUrl)
            };
            #endregion
        }

        public Task<IRestResponse> InvokeAsync<TResult>(IRestRequest request, object body = null, params string[] urlParameters)
            where TResult : class
        {
            AddRequestHeaders(request.Headers);
            return request.Method switch
            {
                ERestMethod.GET => InvokeGetAsync<TResult>(request, urlParameters),
                ERestMethod.POST => InvokePostAsync<TResult>(request, body, urlParameters),
                ERestMethod.PUT => InvokePutAsync<TResult>(request, body, urlParameters),
                ERestMethod.DELETE => InvokeDeleteAsync<TResult>(request, urlParameters),

                _ => throw new NotSupportedException($"Method {request.Method} is not supported"),
            };
        }
        #endregion

        #region invocation methods
        private async Task<IRestResponse> InvokeGetAsync<TResult>(IRestRequest request, params string[] urlParameters)
            where TResult : class
        {
            var urlParam = urlParameters.Length != 0 ? $"?{string.Join("&", urlParameters)}" : string.Empty;
            try
            {
                var result = await _httpClient.GetAsync(string.Concat(request.Route, urlParam));
                if (result is null)
                {
                    return NullResponse();
                }
                return await BuildResponse<TResult>(result);
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex);
            }
        }
        private async Task<IRestResponse> InvokePostAsync<TResult>(IRestRequest request, object body = null, params string[] urlParameters)
            where TResult : class
        {
            var urlParam = urlParameters.Length != 0 ? $"?{string.Join("&", urlParameters)}" : string.Empty;
            var content = (HttpContent)null;
            try
            {
                if (body is not null)
                {
                    content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
                }

                var result = await _httpClient.PostAsync(string.Concat(request.Route, urlParam), content);
                if (result is null)
                {
                    return NullResponse();
                }
                return await BuildResponse<TResult>(result);
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex);
            }
        }
        private async Task<IRestResponse> InvokePutAsync<TResult>(IRestRequest request, object body = null, params string[] urlParameters)
            where TResult : class
        {
            var urlParam = urlParameters.Length != 0 ? $"?{string.Join("&", urlParameters)}" : string.Empty;
            var content = (HttpContent)null;
            try
            {
                if (body is not null)
                {
                    content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
                }

                var result = await _httpClient.PutAsync(string.Concat(request.Route, urlParam), content);
                if (result is null)
                {
                    return NullResponse();
                }
                return await BuildResponse<TResult>(result);
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex);
            }
        }
        private async Task<IRestResponse> InvokeDeleteAsync<TResult>(IRestRequest request, params string[] urlParameters)
            where TResult : class
        {
            var urlParam = urlParameters.Length != 0 ? $"?{string.Join("&", urlParameters)}" : string.Empty;
            try
            {
                var result = await _httpClient.DeleteAsync(string.Concat(request.Route, urlParam));
                if (result is null)
                {
                    return NullResponse();
                }
                return await BuildResponse<TResult>(result);
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex);
            }
        }
        #endregion

        #region response helper
        private IRestResponse NullResponse()
        {
            return new FinalRestResponse
            {
                Exception = new ArgumentNullException("Response is null"),
            };
        }
        private IRestResponse ExceptionResponse(Exception ex, HttpStatusCode? statusCode = null)
        {
            return new FinalRestResponse
            {
                Exception = ex,
                Reason = ex.Message,
                StatusCode = statusCode ?? 0,
            };
        }
        private async Task<IRestResponse> BuildResponse<TResult>(HttpResponseMessage message)
            where TResult : class
        {
            try
            {
                var json = await message.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<TResult>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return new FinalRestResponse
                {
                    Data = data,
                    StatusCode = message.StatusCode,
                    Reason = message.ReasonPhrase,
                };
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex, message.StatusCode);
            }
        }

        #endregion

        #region request helper
        private void AddRequestHeaders(FinalRestHeaderCollection headers)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
        #endregion

    }

}
