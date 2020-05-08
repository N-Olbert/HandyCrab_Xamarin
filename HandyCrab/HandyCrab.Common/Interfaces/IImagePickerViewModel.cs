using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HandyCrab.Common.Interfaces
{
    /// <summary>
    /// Interface for the image picker view model
    /// </summary>
    public interface IImagePickerViewModel : IViewModel
    {
        /// <summary>
        /// Gets a command which takes an image asynchronous.
        /// </summary>
        ICommand TakeImageAsync { get; }

        /// <summary>
        /// Gets a command which selects an image asynchronous.
        /// </summary>
        ICommand SelectImageAsync { get; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        ImageSource Image { get; }
    }
}