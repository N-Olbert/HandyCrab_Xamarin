using System;
using System.Net.Http;
using System.Threading.Tasks;
using HandyCrab.Common.Entitys;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public class RegisterClient : BaseClient, IRegisterClient
    {
        public RegisterClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<Failable<User>> RegisterAsync(UserWithPassword user)
        {
            const string settingsName = "RegisterClientBaseAddress";
            try
            {
                var uri = AssemblyConfig<RegisterClient>.GetValue(settingsName);

                var response = await Client.PostAsync(new Uri(uri), user.ToJsonContent());
                var result = await HandleResponseAsync<User>(response);
                if (result.IsSucceeded())
                {
                    await UpdateSessionCookieAsync(response);
                }

                await UpdateCurrentUserAsync(result.Value);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Failable<User>(e);
            }
        }
    }
}