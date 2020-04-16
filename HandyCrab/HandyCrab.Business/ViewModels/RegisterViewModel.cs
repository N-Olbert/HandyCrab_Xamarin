using System;
using System.Windows.Input;
using HandyCrab.Business.Services;
using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;
using JetBrains.Annotations;
using Xamarin.Forms;

namespace HandyCrab.Business.ViewModels
{
    internal class RegisterViewModel : LoginViewModel, IRegisterViewModel
    {
        public string EmailPlaceholder => "E-Mail";
        public new string UserNamePlaceholder => "Benutzername";
        public new string PasswordPlaceholder => "Passwort";
        public string SignUpButtonText => "Registrieren";
        private string email;
        [NotNull]
        private readonly Command registerCommand;
        public event EventHandler RegisterSucceeded;

        public event EventHandler<Exception> RegisterRejected;

        public string Email
        {
            get => this.email;
            set
            {
                SetProperty(ref this.email, value);
                this.registerCommand.ChangeCanExecute();
            }
        }

        public ICommand RegisterCommand => this.registerCommand;

        public RegisterViewModel()
        {
            this.registerCommand = new Command(RegisterAction, CanExecuteRegisterAction);
            LoginCommand.CanExecuteChanged += (sender, args) => this.registerCommand.ChangeCanExecute();
            PropertyChanged += (sender, args) =>
            {
                if (args?.PropertyName == nameof(IsBusy))
                {
                    this.registerCommand.ChangeCanExecute();
                }
            };
        }

        protected bool CanExecuteRegisterAction()
        {
            //Todo: use same regex as backend
            return !IsBusy && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(Email);
        }

        private async void RegisterAction()
        {
            //async void is ok here (event handler)
            if (CanExecuteRegisterAction())
            {
                var client = Factory.Get<IRegisterClient>();
                var newUser = new UserWithPassword
                {
                    Username = UserName,
                    Password = Password,
                    Email = Email
                };

                var user = await client.RegisterAsync(newUser);
                if (user.IsSucceeded())
                {
                    RegisterSucceeded?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    RegisterRejected?.Invoke(this, user.ThrownException);
                }
            }

            this.registerCommand.ChangeCanExecute();
        }
    }
}