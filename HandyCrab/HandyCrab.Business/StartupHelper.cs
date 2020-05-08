using HandyCrab.Business.Services;
using HandyCrab.Business.ViewModels;
using HandyCrab.Common;
using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;

namespace HandyCrab.Business
{
    public static class StartupHelper
    {
        public static void Prepare()
        {
            ViewModelFactory.RegisterInstance<IAboutViewModel, AboutViewModel>();
            ViewModelFactory.RegisterInstance<ISearchViewModel, SearchViewModel>();
            ViewModelFactory.RegisterInstance<ILoginViewModel, LoginViewModel>();
            ViewModelFactory.RegisterInstance<IRegisterViewModel, RegisterViewModel>();
            ViewModelFactory.RegisterInstance<ISearchResultsViewModel, SearchResultsViewModel>();
            ViewModelFactory.RegisterInstance<IAddOrModifyBarrierViewModel, AddOrModifyBarrierViewModel>();
            ViewModelFactory.RegisterInstance<IHamburgerMenuMasterViewModel, HamburgerMenuMasterViewModel>();
            ViewModelFactory.RegisterInstance<IMainViewModel, MainViewModel>();
            ViewModelFactory.RegisterInstance<IBarrierViewModel, BarrierViewModel>();
            ViewModelFactory.RegisterInstance<ILocationSelectionViewModel, LocationSelectionViewModel>();
            UsernameResolverSingletonHolder.SetInstance(Factory.Get<IUserClient>());
        }
    }
}