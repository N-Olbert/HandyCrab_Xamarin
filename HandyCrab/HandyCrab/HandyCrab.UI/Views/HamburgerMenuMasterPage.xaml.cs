using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCrab.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HamburgerMenuMasterPage : ContentPage
    {
        public HamburgerMenuMasterPage()
        {
            InitializeComponent();
            listView.ItemSelected += OnItemSelected;
            Title = "Hamburger";
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
                masterPage.Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                listView.SelectedItem = null;
                masterPage.IsPresented = false;
            }
        }
    }
}