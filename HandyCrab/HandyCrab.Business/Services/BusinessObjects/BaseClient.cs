using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using HandyCrab.Common.Entitys;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public class BaseClient
    {
        [NotNull]
        protected HttpClient Client { get; }

        protected BaseClient(HttpClient httpClient)
        {
            this.Client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        protected async Task<Failable<T>> HandleResponseAsync<T>(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var obj = response.Content != null
                        ? (await response.Content.ReadAsStringAsync()).DeserializeJson<T>()
                        : default;
                    return new Failable<T>(obj, response.StatusCode);
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.NotFound:
                    var errorCode = response.Content != null
                        ? (await response.Content.ReadAsStringAsync()).DeserializeJson<ErrorResponse>()
                        : default;
                    return errorCode != null
                        ? new Failable<T>(default, errorCode.ErrorCode, response.StatusCode)
                        : new Failable<T>(default, response.StatusCode);
                default:
                    var exception = new ArgumentOutOfRangeException(nameof(response.StatusCode), "Invalid status code form backend.");
                    return new Failable<T>(default, response.StatusCode, exception);
            }
        }

        protected async Task UpdateSessionCookieAsync(HttpResponseMessage message)
        {
            var cookie = GetCookie(message);
            if (string.IsNullOrEmpty(cookie))
            {
                throw new InvalidOperationException("No cookie found");
            }

            var storageProvider = Factory.Get<ISecureStorage>();
            await storageProvider.StoreAsync(nameof(StorageSlot.CurrentUserCookie), cookie);
        }

        protected async Task UpdateCurrentUserAsync(User user)
        {
            var storageProvider = Factory.Get<ISecureStorage>();
            await storageProvider.StoreAsync(nameof(StorageSlot.CurrentUser), JsonConvert.SerializeObject(user));
        }

        private static string GetCookie(HttpResponseMessage message)
        {
            //based on: https://stackoverflow.com/a/58947897/1676819
            if (message?.Headers != null &&
                message.Headers.TryGetValues("Set-Cookie", out var setCookie))
            {
                var setCookieString = setCookie.SingleOrDefault();
                var cookieTokens = setCookieString?.Split(';');
                var firstCookie = cookieTokens?.FirstOrDefault();
                var keyValueTokens = firstCookie?.Split('=');
                if (keyValueTokens != null && keyValueTokens.Length > 0)
                {
                    var valueString = keyValueTokens[1];
                    var cookieValue = HttpUtility.UrlDecode(valueString);
                    return cookieValue;
                }
            }

            return string.Empty;
        }

        private class ErrorResponse
        {
            [JsonProperty("errorCode")]
            public int ErrorCode { get; set; }
        }
    }
}