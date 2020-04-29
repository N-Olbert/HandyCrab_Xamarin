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
        /// Gets the search results view model.
        /// </summary>
        public static ISearchResultsViewModel SearchResultsViewModel => ViewModelFactory.GetInstance<ISearchResultsViewModel>();

        /// <summary>
        /// Gets the about view model.
        /// </summary>
        public static IAboutViewModel AboutViewModel => ViewModelFactory.GetInstance<IAboutViewModel>();

        /// <summary>
        /// Gets the image picker view model.
        /// </summary>
        public static IImagePickerViewModel ImagePickerViewModel => ViewModelFactory.GetInstance<IImagePickerViewModel>();

        /// <summary>
        /// Gets the login view model.
        /// </summary>
        public static ILoginViewModel LoginViewModel => ViewModelFactory.GetInstance<ILoginViewModel>();

        /// <summary>
        /// Gets the register view model.
        /// </summary>
        public static IRegisterViewModel RegisterViewModel => ViewModelFactory.GetInstance<IRegisterViewModel>();

        /// <summary>
        /// Gets the add or modify barrier view model.
        /// </summary>
        public static IAddOrModifyBarrierViewModel AddOrModifyBarrierViewModel => ViewModelFactory.GetInstance<IAddOrModifyBarrierViewModel>();

        /// <summary>
        /// Gets the hamburger menu view model.
        /// </summary>
        public static IHamburgerMenuMasterViewModel HamburgerMenuMasterViewModel => ViewModelFactory.GetInstance<IHamburgerMenuMasterViewModel>();
    }
}