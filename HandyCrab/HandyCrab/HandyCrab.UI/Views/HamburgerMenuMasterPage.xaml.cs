using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using HandyCrab.Common.Interfaces;
using HandyCrab.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HamburgerMenuMasterPage : ContentPage
    {
        private ObservableCollection<MasterPageItem> MenuWhileNotLoggedIn = new ObservableCollection<MasterPageItem>();

        private ObservableCollection<MasterPageItem> MenuWhileLoggedIn = new ObservableCollection<MasterPageItem>(); 

        public HamburgerMenuMasterPage()
        {
            InitializeComponent();
            listView.ItemSelected += OnItemSelected;
            Title = "Hamburger";

            MenuWhileNotLoggedIn.Add(new MasterPageItem { Title = "Login", TargetType = typeof(LoginPage) });
            MenuWhileNotLoggedIn.Add(new MasterPageItem { Title = "Registrieren", TargetType = typeof(RegisterPage) });
            MenuWhileNotLoggedIn.Add(new MasterPageItem { Title = "About", TargetType = typeof(AboutPage) });

            MenuWhileLoggedIn.Add(new MasterPageItem { Title = "Suche", TargetType = typeof(SearchPage) });
            MenuWhileLoggedIn.Add(new MasterPageItem { Title = "About", TargetType = typeof(AboutPage) });

            var vm = (IHamburgerMenuMasterViewModel)BindingContext;

            listView.ItemsSource = vm.CurrentUser == null ? MenuWhileNotLoggedIn : MenuWhileLoggedIn;

            vm.OnLoginStatusChanged += (sender, args) => listView.ItemsSource = vm.CurrentUser == null ? MenuWhileNotLoggedIn : MenuWhileLoggedIn;
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