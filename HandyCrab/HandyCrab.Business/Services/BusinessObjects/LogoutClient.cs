using System;
using System.Net.Http;
using System.Threading.Tasks;
using HandyCrab.Common.Entitys;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public class LogoutClient : BaseClient, ILogoutClient
    {
        public LogoutClient(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <inheritdoc />
        public async Task<Failable> LogoutAsync()
        {
            const string settingsName = "LogoutClientBaseAddress";
            try
            {
                var uri = AssemblyConfig<RegisterClient>.GetValue(settingsName);
                var message = await GetHttpMessageWithSessionCookie(uri, HttpMethod.Get);
                var response = await Client.SendAsync(message);
                var result = await HandleResponseAsync<NoReturnValue>(response);

                //Delete anyways -- even if result failed
                await UpdateCookiesAsync(null);
                await UpdateCurrentUserAsync(null);
                if (!result.IsSucceeded())
                {
                    throw new InvalidOperationException(result.ThrownException?.Message ?? "Logout rejected by backend");
                }

                return result;
            }
            catch (Exception e)
            {
                return new Failable(e);
            }
        }
    }
}