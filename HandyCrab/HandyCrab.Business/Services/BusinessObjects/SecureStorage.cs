using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public class SecureStorage : ISecureStorage
    {
        private static readonly Lazy<ConcurrentDictionary<string, string>> iOSEmulatorStorageInstance = new Lazy<ConcurrentDictionary<string, string>>();
        private static ConcurrentDictionary<string, string> iOSEmulatorStorage => iOSEmulatorStorageInstance.Value;
        public async Task StoreAsync(string key, string value)
        {
            if(Xamarin.Essentials.DeviceInfo.Platform == Xamarin.Essentials.DevicePlatform.iOS &&
               Xamarin.Essentials.DeviceInfo.DeviceType == Xamarin.Essentials.DeviceType.Virtual)
            {
                iOSEmulatorStorage.AddOrUpdate(key, value, (currentKey, oldValue) => value);
                return;
            }

            await Xamarin.Essentials.SecureStorage.SetAsync(key, value);
        }

        public async Task<string> GetAsync(string key)
        {
            if (Xamarin.Essentials.DeviceInfo.Platform == Xamarin.Essentials.DevicePlatform.iOS &&
                Xamarin.Essentials.DeviceInfo.DeviceType == Xamarin.Essentials.DeviceType.Virtual)
            {
                iOSEmulatorStorage.TryGetValue(key, out var value);
                return value;
            }

            return await Xamarin.Essentials.SecureStorage.GetAsync(key);
        }
    }
}