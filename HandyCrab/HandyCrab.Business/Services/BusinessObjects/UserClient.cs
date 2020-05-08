using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HandyCrab.Common.Interfaces;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public class UserClient : BaseClient, IUserClient
    {
        [NotNull]
        private static readonly ConcurrentDictionary<string, string> UsernameCache = new ConcurrentDictionary<string, string>();
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
        public UserClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            if (UsernameCache.TryGetValue(userId, out var userName))
            {
                return userName;
            }

            const string settingsName = "UserClientGetNameBaseAddress";
            const string unknownUser = "??";

            await Semaphore.WaitAsync();
            try
            {
                //double check
                if (UsernameCache.TryGetValue(userId, out var username))
                {
                    return username;
                }

                var uri = AssemblyConfig<RegisterClient>.GetValue(settingsName);

                var data = new RequestUsernameData {Id = userId};
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(uri),
                    Content = data.ToJsonContent(),
                };

                var response = await Client.SendAsync(request);
                var result = await HandleResponseAsync<RequestUsernameData>(response);

                //No errorhandling here (non vital action)
                UsernameCache.TryAdd(userId, result?.Value.Result ?? unknownUser);
                return result?.Value.Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UsernameCache.TryAdd(userId, unknownUser);
                return unknownUser;
            }
            finally
            {
                Semaphore.Release();
            }
        }

        private struct RequestUsernameData
        {
            [JsonProperty("_id")]
            public string Id;

            [JsonProperty("result")]
            public string Result;
        }
    }
}