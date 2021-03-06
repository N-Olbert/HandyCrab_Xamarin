﻿using HandyCrab.Common.Interfaces;
using HandyCrab.Common.Entitys;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : BaseContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            var vm = (IRegisterViewModel)BindingContext;

            //Bind vm events
            vm.RegisterSucceeded += (sender, args) => NavigationHelper.GoTo(new SearchPage());
            vm.RegisterRejected += onRegisterRejected;
            vm.OnError += OnError;
        }

        private void onRegisterRejected(object sender, Failable e)
        {
            switch (e.ErrorCode)
            {
                case 3:
                    DisplayAlert("Fehler", Strings.Error_MailAlreadyInUse, "OK");
                    break;
                case 4:
                    DisplayAlert("Fehler", Strings.Error_UsernameAlreadyInUse, "OK");
                    break;
                case 5:
                    DisplayAlert("Fehler", Strings.Error_InvalidMail, "OK");
                    break;
                case 12:
                    DisplayAlert("Fehler", Strings.Error_InvalidUsername, "OK");
                    break;
                case 13:
                    DisplayAlert("Fehler", Strings.Error_InvalidPassword, "OK");
                    break;
                default:
                    OnError(sender, e);
                    break;
            }
        }
    }
}