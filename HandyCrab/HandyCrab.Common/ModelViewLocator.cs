using HandyCrab.Common.Interfaces;

namespace HandyCrab.Common
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
    }
}