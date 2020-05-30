using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultsPage : BaseContentPage
    {
        public SearchResultsPage()
        {
            InitializeComponent();
            var vm = (ISearchResultsViewModel)BindingContext;

            vm.OnError += OnError;
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
    }
}