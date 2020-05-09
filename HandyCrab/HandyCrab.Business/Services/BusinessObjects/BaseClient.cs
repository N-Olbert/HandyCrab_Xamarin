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

        protected async Task<HttpRequestMessage> GetHttpMessageWithSessionCookie(string uri, HttpMethod httpMethod)
        {
            var cookie = await GetHttpMessageCookiesAsync();
            var message = new HttpRequestMessage(httpMethod, uri);
            message.Headers.Add("Cookie", cookie);
            return message;
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
                    await UpdateCookieAsync(response, CookieType.Session, true);
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
                        : new Failable<T>(default, Failable.GenericErrorCode, response.StatusCode);
                default:
                    var exception = new ArgumentOutOfRangeException(nameof(response.StatusCode), "Invalid status code form backend.");
                    return new Failable<T>(default, response.StatusCode, exception);
            }
        }

        protected async Task UpdateCookiesAsync(HttpResponseMessage message)
        {
            await UpdateCookieAsync(message, CookieType.Session, false);
            await UpdateCookieAsync(message, CookieType.Token, false);
        }

        protected async Task UpdateCurrentUserAsync(User user)
        {
            var storageProvider = Factory.Get<ISecureStorage>();
            await storageProvider.StoreAsync(nameof(SecureStorageSlot.CurrentUser), JsonConvert.SerializeObject(user));
        }

        protected async Task<string> GetHttpMessageCookiesAsync()
        {
            var storageProvider = Factory.Get<ISecureStorage>();
            var cookie = $"{await storageProvider.GetAsync(nameof(SecureStorageSlot.CurrentUserSessionCookie))}";
            var tokenCookie = await storageProvider.GetAsync(nameof(SecureStorageSlot.CurrentUserTokenCookie));
            if (!string.IsNullOrEmpty(tokenCookie))
            {
                cookie = $"{cookie}; {tokenCookie}";
            }

            return cookie;
        }

        private async Task UpdateCookieAsync(HttpResponseMessage message, CookieType type, bool storeOnlyNonNull)
        {
            var cookie = GetCookie(message, type);
            if (storeOnlyNonNull && string.IsNullOrEmpty(cookie))
            {
                return;
            }

            var storageProvider = Factory.Get<ISecureStorage>();
            switch (type)
            {
                case CookieType.Session:
                    await storageProvider.StoreAsync(nameof(SecureStorageSlot.CurrentUserSessionCookie), cookie);
                    break;
                case CookieType.Token:
                    await storageProvider.StoreAsync(nameof(SecureStorageSlot.CurrentUserTokenCookie), cookie);
                    break;
                default: return;
            }
        }

        private static string GetCookie(HttpResponseMessage message, CookieType type)
        {
            //based on: https://stackoverflow.com/a/58947897/1676819
            if (message?.Headers != null &&
                message.Headers.TryGetValues("Set-Cookie", out var setCookie))
            {
                string cookieName;
                switch (type)
                {
                    case CookieType.Session:
                        cookieName = "JSESSION";
                        break;
                    case CookieType.Token:
                        cookieName = "TOKEN";
                        break;
                    default: return null;
                }

                var setCookieString = setCookie.SingleOrDefault(x => x != null && x.StartsWith(cookieName));
                var cookieTokens = setCookieString?.Split(';');
                return cookieTokens?.FirstOrDefault();
            }

            return string.Empty;
        }

        private class ErrorResponse
        {
            [JsonProperty("errorCode")]
            public int ErrorCode { get; set; }
        }

        private enum CookieType
        {
            Session,
            Token
        }
    }
}