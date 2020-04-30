using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using HandyCrab.Common.Entitys;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultsPage : ContentPage
    {
        public SearchResultsPage()
        {
            InitializeComponent();
        }

        void OnAddBarrierButtonClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new AddOrModifyBarrierPage());
        }

        void onItemTapped(object sender, EventArgs args)
        {
            string id = ((Barrier)((ListView)sender).SelectedItem).Id;
            Navigation.PushAsync(new BarrierPage(id));
        }
    }
}