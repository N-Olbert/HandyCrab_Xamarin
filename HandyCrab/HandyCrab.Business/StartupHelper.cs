using HandyCrab.Business.ViewModels;
using HandyCrab.Common;
using HandyCrab.Common.Interfaces;

namespace HandyCrab.Business
{
    public static class StartupHelper
    {
        public static void Prepare()
        {
            ViewModelFactory.RegisterInstance<IAboutViewModel, AboutViewModel>();
            ViewModelFactory.RegisterInstance<ISearchViewModel, SearchViewModel>();
            ViewModelFactory.RegisterInstance<IImagePickerViewModel, ImagePickerViewModel>();
            ViewModelFactory.RegisterInstance<ILoginViewModel, LoginViewModel>();
            ViewModelFactory.RegisterInstance<IRegisterViewModel, RegisterViewModel>();
            ViewModelFactory.RegisterInstance<ISearchResultsViewModel, SearchResultsViewModel>();
        }
    }
}