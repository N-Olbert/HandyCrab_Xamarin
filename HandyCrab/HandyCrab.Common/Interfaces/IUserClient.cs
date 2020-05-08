using System.Threading.Tasks;

namespace HandyCrab.Common.Interfaces
{
    public interface IUserNameResolver
    {
        Task<string> GetUserNameAsync(string userId);
    }
}