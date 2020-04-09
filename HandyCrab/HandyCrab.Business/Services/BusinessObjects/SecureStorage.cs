using System;
using System.Threading.Tasks;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public class SecureStorage : ISecureStorage
    {
        public async Task StoreAsync(string key, string value)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(key, value);
        }

        public async Task<string> GetAsync(string key)
        {
            return await Xamarin.Essentials.SecureStorage.GetAsync(key);
        }
    }
}