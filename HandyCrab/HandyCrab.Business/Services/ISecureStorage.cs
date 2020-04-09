using System.Threading.Tasks;
using JetBrains.Annotations;

namespace HandyCrab.Business.Services
{
    public interface ISecureStorage
    {
        [NotNull]
        Task StoreAsync(string key, string value);

        Task<string> GetAsync(string key);
    }
}