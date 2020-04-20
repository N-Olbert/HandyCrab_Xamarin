using System;
using System.Text.RegularExpressions;
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
        private string email;
        [NotNull]
        private readonly Command registerCommand;
        public event EventHandler RegisterSucceeded;

        public event EventHandler<Failable> RegisterRejected;

        public string Email
        {
            get => this.email;
            set
            {
                SetProperty(ref this.email, value?.TrimEnd(' '));
                RaisePropertyChanged(nameof(IsEmailValid));
                this.registerCommand.ChangeCanExecute();
            }
        }

        public string EmailValidationRegex => "^[\\w!#$%&'*+/=?`{|}~^-]+(?:\\.[\\w!#$%&'*+/=?`{|}~^-]+)*@(?:[a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$";

        public bool IsEmailValid => !string.IsNullOrEmpty(Email) && Regex.IsMatch(Email, EmailValidationRegex, RegexOptions.Compiled);

        public override string UserNameValidationRegex => "[a-zA-Z0-9_]{4,16}";

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
                    RegisterRejected?.Invoke(this, user);
                }
            }

            this.registerCommand.ChangeCanExecute();
        }
    }
}