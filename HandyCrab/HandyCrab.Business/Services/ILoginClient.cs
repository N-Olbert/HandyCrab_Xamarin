using System.Threading.Tasks;
using HandyCrab.Common.Entitys;
using JetBrains.Annotations;

namespace HandyCrab.Business.Services
{
    public interface ILoginClient
    {
        [NotNull]
        Task<Failable<User>> LoginAsync(Login user);
    }
}