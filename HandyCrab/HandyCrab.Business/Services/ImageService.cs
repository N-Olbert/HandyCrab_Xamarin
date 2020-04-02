using System.Threading.Tasks;
using HandyCrab.Business.ViewModels;
using HandyCrab.Common.Interfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace HandyCrab.Business.Services
{
    internal static class ImageService
    {
        public static async Task<ImageSource> TakeImageAsync()
        {
            var mediaProvider = CrossMedia.Current;
            if (mediaProvider != null)
            {
                await mediaProvider.Initialize();
                if (mediaProvider.IsTakePhotoSupported)
                {
                    var options = new StoreCameraMediaOptions() {SaveToAlbum = false};
                    var pic = await mediaProvider.TakePhotoAsync(options);

                    return pic == null ? null : ImageSource.FromStream(pic.GetStream);
                }
            }

            return null;
        }

        public static async Task<ImageSource> SelectImageAsync()
        {
            var mediaProvider = CrossMedia.Current;
            if (mediaProvider != null)
            {
                await mediaProvider.Initialize();
                if (mediaProvider.IsPickPhotoSupported)
                {
                    var options = new PickMediaOptions();
                    var pic = await mediaProvider.PickPhotoAsync(options);

                    return pic == null ? null : ImageSource.FromStream(pic.GetStream);
                }
            }

            return null;
        }
    }
}