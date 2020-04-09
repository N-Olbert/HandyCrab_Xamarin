using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCrab.Common.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            var vm = (ILoginViewModel) BindingContext;
            vm.LoginSucceeded += (sender, args) =>
            {
                MasterDetailPage MasterPage = App.Current.MainPage as MasterDetailPage;
                MasterPage.Detail = new NavigationPage(new SearchPage());
                MasterPage.IsPresented = false;
            };

            vm.LoginRejected += (sender, s) => DisplayAlert("Alert", s, "OK");
        }

        private void NoAccountButton_Clicked(object sender, EventArgs e)
        {
            //TODO: send data to the backend for verification, only redirect to search page if successful
            MasterDetailPage MasterPage = App.Current.MainPage as MasterDetailPage;
            MasterPage.Detail = new NavigationPage(new RegisterPage());
            MasterPage.IsPresented = false;
        }
    }
}