using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCrab.Common;
using HandyCrab.Common.Interfaces;
using HandyCrab.Common.Entitys;
using HandyCrab.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
            var vm = (ISearchViewModel)BindingContext;

            vm.SearchSucceeded += (sender, args) => Navigation.PushAsync(new SearchResultsPage());
            vm.OnError += (sender, args) => OnError(sender, args);

            IdToBoolConverter.userId = vm.CurrentUser?.Id;
        }

        private void ChangeLocationButton_OnClicked(object sender, EventArgs e)
        {
            NavigationHelper.GoTo(new LocationMapPage());
        }

        private void OnError(object sender, Failable e)
        {
            switch(e.ErrorCode)
            {
                case 1:
                    DisplayAlert("Fehler", Strings.Error_UnknownError, "OK");
                    break;
                case 2:
                    DisplayAlert("Fehler", Strings.Error_NotLoggedIn, "OK");
                    break;
                case 2147483647:
                    DisplayAlert("Fehler", Strings.Error_NetworkTimeout, "OK");
                    break;
                default:
                    DisplayAlert("Fehler", Strings.Error_UnknownError, "OK");
                    break;
            }
        }
    }
}