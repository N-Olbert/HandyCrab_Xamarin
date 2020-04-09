using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using HandyCrab.Common.Entitys;
using JetBrains.Annotations;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public class BaseClient
    {
        private const string CookieKey = "Cookie";

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

                default:
                    //TODO
                    throw new ArgumentOutOfRangeException();
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
            await storageProvider.StoreAsync(CookieKey, cookie);
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
    }
}