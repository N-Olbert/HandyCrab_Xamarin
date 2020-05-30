using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HandyCrab.Common.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : BaseContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
            var vm = (ISearchViewModel)BindingContext;

            vm.SearchSucceeded += (sender, args) => Navigation.PushAsync(new SearchResultsPage());
            vm.OnError += OnError;

            IdToBoolConverter.userId = vm.CurrentUser?.Id;
            this.refreshView.RefreshCommand = new Command(() =>
            {
                vm.UpdateCurrentGeolocationAsync();
            });
        }

        private void ChangeLocationButton_OnClicked(object sender, EventArgs e)
        {
            NavigationHelper.GoTo(new LocationMapPage());
        }
    }
}