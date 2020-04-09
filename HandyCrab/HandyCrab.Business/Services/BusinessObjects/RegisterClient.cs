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

                var res = await Client.PostAsync(new Uri(uri), user.ToJsonContent());
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