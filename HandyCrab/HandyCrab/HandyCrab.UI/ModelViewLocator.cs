using HandyCrab.Common;
using HandyCrab.Common.Interfaces;

namespace HandyCrab.UI
{
    /// <summary>
    /// Helper for enforcing "Model View Locator"-Pattern
    /// </summary>
    public class ModelViewLocator
    {
        /// <summary>
        /// Gets the search view model.
        /// </summary>
        public static ISearchViewModel SearchViewModel => ViewModelFactory.GetInstance<ISearchViewModel>();

        /// <summary>
        /// Gets the about view model.
        /// </summary>
        public static IAboutViewModel AboutViewModel => ViewModelFactory.GetInstance<IAboutViewModel>();

        /// <summary>
        /// Gets the image picker view model.
        /// </summary>
        public static IImagePickerViewModel ImagePickerViewModel => ViewModelFactory.GetInstance<IImagePickerViewModel>();

        /// <summary>
        /// Gets the image picker view model.
        /// </summary>
        public static ILoginViewModel LoginViewModel => ViewModelFactory.GetInstance<ILoginViewModel>();
    }
}