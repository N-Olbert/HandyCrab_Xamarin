using System;
using HandyCrab.Common.Interfaces;
using HandyCrab.Common.Entitys;
using Xamarin.Forms.Xaml;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : BaseContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            var vm = (ILoginViewModel) BindingContext;

            //Bind vm events
            vm.LoginSucceeded += (sender, args) => NavigationHelper.GoTo(new SearchPage());
            vm.LoginRejected += new EventHandler<Failable>(OnLoginRejected);
            vm.OnError += OnError;
        }

        private void NoAccountButton_Clicked(object sender, EventArgs e)
        {
            NavigationHelper.GoTo(new RegisterPage());
        }

        private void OnLoginRejected(object sender, Failable e)
        {
            switch(e.ErrorCode)
            {
                case 6:
                    DisplayAlert("Fehler", Strings.Error_WrongLogin, "OK");
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