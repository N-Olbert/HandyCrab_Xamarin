using System;
using System.Windows.Input;
using HandyCrab.Business.Services;
using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;
using JetBrains.Annotations;
using Xamarin.Forms;

namespace HandyCrab.Business.ViewModels
{
    internal class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        public string UserNamePlaceholder => "E-Mail oder Benutzername";
        public string PasswordPlaceholder => "Passwort";
        public string LoginButtonText => "Login";
        public string NoAccountButtonText => "Ich habe noch keinen Account";
        private string userName;
        private string passWord;
        [NotNull]
        private readonly Command loginCommand;

        public event EventHandler LoginSucceeded;

        public event EventHandler<Exception> LoginRejected;

        public string UserName
        {
            get => this.userName;
            set
            {
                SetProperty(ref this.userName, value);
                this.loginCommand.ChangeCanExecute();
            }
        }

        /*[System.ComponentModel.DefaultValue("Test")]
        public string UserNamePlaceholder
        {
            get => "Test";
            set
            {
                SetProperty(ref this.userNamePlaceholder, "E-Mail2 oder Benutzername");
            }
        }*/

        public string Password
        {
            get => this.passWord;
            set
            {
                SetProperty(ref this.passWord, value);
                this.loginCommand.ChangeCanExecute();
            }
        }

        [NotNull]
        public ICommand LoginCommand
        {
            get => this.loginCommand;
        }

        public LoginViewModel()
        {
            this.loginCommand = new Command(LoginAction, CanExecuteLoginAction);
            PropertyChanged += (sender, args) =>
            {
                if (args?.PropertyName == nameof(IsBusy))
                {
                    this.loginCommand.ChangeCanExecute();
                }
            };
        }

        private bool CanExecuteLoginAction()
        {
            //Todo: use same regex as backend
            return !IsBusy && !string.IsNullOrEmpty(this.userName) && !string.IsNullOrEmpty(this.passWord);
        }

        private async void LoginAction()
        {
            //async void is ok here (event handler)
            if (CanExecuteLoginAction())
            {
                var client = Factory.Get<ILoginClient>();
                var login = new Login
                {
                    Username = UserName,
                    Password = Password
                };

                var user = await client.LoginAsync(login);
                if (user.IsSucceeded())
                {
                    LoginSucceeded?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    LoginRejected?.Invoke(this, user.ThrownException);
                }
            }

            this.loginCommand.ChangeCanExecute();
        }
    }
}