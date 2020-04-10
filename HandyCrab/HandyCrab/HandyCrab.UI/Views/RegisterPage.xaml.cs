using System;
using HandyCrab.Common.Interfaces;
using HandyCrab.UI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            var vm = (IRegisterViewModel)BindingContext;

            //Bind vm events
            vm.RegisterSucceeded += (sender, args) => NagivationHelper.GoTo(new SearchPage());
            vm.RegisterRejected += (sender, s) => DisplayAlert("Alert", s, "OK");
        }
    }
}