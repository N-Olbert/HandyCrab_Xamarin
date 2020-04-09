using System.Threading.Tasks;
using HandyCrab.Common.Entitys;

namespace HandyCrab.Business.Services
{
    public interface ILoginClient
    {
        Task<Failable<User>> LoginAsync(Login user);
    }
}