using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace HandyCrab.Business.Services.BusinessObjects
{
    internal class ImageService : IImageService
    {
        public async Task<ImageSource> TakeImageAsync()
        {
            var mediaProvider = CrossMedia.Current;
            if (mediaProvider != null)
            {
                await mediaProvider.Initialize();
                if (mediaProvider.IsTakePhotoSupported)
                {
                    var options = new StoreCameraMediaOptions()
                    {
                        SaveToAlbum = false,
                        
                        //Autosize too large images for backend
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 1500,
                        CompressionQuality = 90
                    };
                    var pic = await mediaProvider.TakePhotoAsync(options);

                    return pic == null ? null : ImageSource.FromStream(pic.GetStream);
                }
            }

            return null;
        }

        public async Task<ImageSource> SelectImageAsync()
        {
            var mediaProvider = CrossMedia.Current;
            if (mediaProvider != null)
            {
                await mediaProvider.Initialize();
                if (mediaProvider.IsPickPhotoSupported)
                {
                    var options = new PickMediaOptions
                    {
                        //Autosize too large images for backend
                        PhotoSize = PhotoSize.MaxWidthHeight, 
                        MaxWidthHeight = 1500, 
                        CompressionQuality = 90
                    };
                    var pic = await mediaProvider.PickPhotoAsync(options);

                    return pic == null ? null : ImageSource.FromStream(pic.GetStream);
                }
            }

            return null;
        }
    }
}