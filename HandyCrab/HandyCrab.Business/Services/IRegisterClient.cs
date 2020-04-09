using System.Threading.Tasks;
using HandyCrab.Common.Entitys;
using JetBrains.Annotations;

namespace HandyCrab.Business.Services
{
    public interface IRegisterClient
    {
        [NotNull]
        Task<Failable<User>> RegisterAsync(UserWithPassword user);
    }
}