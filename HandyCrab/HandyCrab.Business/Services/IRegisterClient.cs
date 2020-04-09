using System.Threading.Tasks;
using HandyCrab.Common.Entitys;

namespace HandyCrab.Business.Services
{
    public interface IRegisterClient
    {
        Task<Failable<User>> RegisterAsync(UserWithPassword user);
    }
}