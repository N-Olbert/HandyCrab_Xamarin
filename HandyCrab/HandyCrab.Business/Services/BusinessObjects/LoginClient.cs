using System;
using System.Net.Http;
using System.Threading.Tasks;
using HandyCrab.Common.Entitys;
using Newtonsoft.Json;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public class LoginClient : BaseClient, ILoginClient
    {
        public LoginClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<Failable<User>> LoginAsync(Login login)
        {
            const string settingsName = "LoginClientBaseAddress";
            try
            {
                var uri = AssemblyConfig<RegisterClient>.GetValue(settingsName);

                var res = await Client.PostAsync(new Uri(uri), login.ToJsonContent());
                await UpdateSessionCookieAsync(res);
                return await HandleResponseAsync<User>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Failable<User>(e);
            }
        }
    }
}