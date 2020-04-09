using System;
using HandyCrab.Common.Interfaces;
using HandyCrab.UI;
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

            //Bind vm events
            vm.LoginSucceeded += (sender, args) => NagivationHelper.GoTo(new SearchPage());
            vm.LoginRejected += (sender, s) => DisplayAlert("Alert", s, "OK");
        }

        private void NoAccountButton_Clicked(object sender, EventArgs e)
        {
            NagivationHelper.GoTo(new RegisterPage());
        }
    }
}