using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultsPage : ContentPage
    {
        public SearchResultsPage()
        {
            InitializeComponent();
            var vm = (ISearchResultsViewModel)BindingContext;

            vm.OnError += (sender, args) => OnError(sender, args);
        }

        void OnAddBarrierButtonClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new AddOrModifyBarrierPage());
        }

        void OnModifyBarrierButtonClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new AddOrModifyBarrierPage(((IReadOnlyBarrier)((Button)sender).BindingContext).Id));
        }

        void onItemTapped(object sender, EventArgs args)
        {
            string id = ((Barrier)((ListView)sender).SelectedItem).Id;
            Navigation.PushAsync(new BarrierPage(id));
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
                case 9:
                    DisplayAlert("Fehler", Strings.Error_BarrierNotFound, "OK");
                    break;
                case 10:
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