using Xamarin.Forms;

namespace HandyCrab.UI
{
    public static class NavigationHelper
    {
        public static void GoTo(Page page)
        {
            var currentMainPage = App.Current.MainPage as MasterDetailPage;
            currentMainPage.Detail = new NavigationPage(page);
            currentMainPage.IsPresented = false;
        }
    }
}