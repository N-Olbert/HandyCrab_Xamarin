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
    internal class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        private string userName;
        private string passWord;
        [NotNull]
        private readonly Command loginCommand;

        public event EventHandler LoginSucceeded;

        public event EventHandler<Failable> LoginRejected;

        public string UserName
        {
            get => this.userName;
            set
            {
                SetProperty(ref this.userName, value?.TrimEnd(' '));
                RaisePropertyChanged(nameof(IsUserNameValid));
                this.loginCommand.ChangeCanExecute();
            }
        }

        public virtual string UserNameValidationRegex => "(^[\\w!#$%&'*+/=?`{|}~^-]+(?:\\.[\\w!#$%&'*+/=?`{|}~^-]+)*@(?:[a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$)|([a-zA-Z0-9_]{4,16})";

        public bool IsUserNameValid =>
            !string.IsNullOrEmpty(UserName) &&
            Regex.IsMatch(UserName, UserNameValidationRegex, RegexOptions.Compiled);
        public string Password
        {
            get => this.passWord;
            set
            {
                SetProperty(ref this.passWord, value);
                RaisePropertyChanged(nameof(IsPasswordValid));
                this.loginCommand.ChangeCanExecute();
            }
        }

        public string PasswordValidationRegex => "[a-zA-Z0-9\"!#$%&'()*+,\\-./:;<=>?@\\[\\]]{6,100}";

        public bool IsPasswordValid =>
            !string.IsNullOrEmpty(Password) &&
            Regex.IsMatch(Password, PasswordValidationRegex, RegexOptions.Compiled);

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
            return !IsBusy && IsUserNameValid && IsPasswordValid;
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
                    LoginRejected?.Invoke(this, user);
                }

                RaiseUserChanged();
            }

            this.loginCommand.ChangeCanExecute();
        }
    }
}