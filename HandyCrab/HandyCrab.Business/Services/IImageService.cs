using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandyCrab.Business.Services
{
    internal interface IImageService
    {
        Task<ImageSource> TakeImageAsync();

        Task<ImageSource> SelectImageAsync();
    }
}