using System.Threading.Tasks;
using HandyCrab.Common.Entitys;
using JetBrains.Annotations;

namespace HandyCrab.Business.Services
{
    /// <summary>
    /// Interface for any logout client
    /// </summary>
    public interface ILogoutClient
    {
        /// <summary>
        /// Logs off the current user (asynchronously).
        /// </summary>
        /// <returns><see cref="Failable"/> which indicates success state.</returns>
        [NotNull]
        Task<Failable> LogoutAsync();
    }
}