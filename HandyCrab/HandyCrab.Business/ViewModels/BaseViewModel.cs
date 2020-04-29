using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HandyCrab.Business.Services;
using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;
using Newtonsoft.Json;

namespace HandyCrab.Business.ViewModels
{
    internal abstract class BaseViewModel : IViewModel
    {
        private static readonly ICommand logoutCommand = new LogoutCommandImpl();
        private bool isBusy;

        /// <summary>
        /// Event which occurs when the singed in user was changed.
        /// Note: It is possible (although unlikely) that the new user is the same user as the old one.
        /// </summary>
        protected static event EventHandler UserChanged;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public event EventHandler<Failable> OnError;

        /// <inheritdoc />
        public bool IsBusy
        {
            get => this.isBusy;
            set => SetProperty(ref this.isBusy, value);
        }

        /// <inheritdoc />
        public User CurrentUser
        {
            get => GetCurrentUser();
        }

        /// <inheritdoc />
        public ICommand LogoutCommand
        {
            get => logoutCommand;
        }

        protected BaseViewModel()
        {
            UserChanged += BaseViewModel_UserChanged;
        }

        private void BaseViewModel_UserChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(CurrentUser));
        }

        protected void SetProperty<T>(ref T backingStore, T value,
                                      [CallerMemberName]string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                backingStore = value;
                RaisePropertyChanged(propertyName);
            }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaiseUserChanged()
        {
            UserChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseOnError(Exception exception)
        {
            OnError?.Invoke(this, new Failable(exception));
        }

        public void RaiseOnError(Failable failable)
        {
            OnError?.Invoke(this, failable);
        }

        private static User GetCurrentUser()
        {
            try
            {
                var jsonUser = Factory.Get<ISecureStorage>().GetAsync(nameof(SecureStorageSlot.CurrentUser))?.GetAwaiter().GetResult();
                return string.IsNullOrEmpty(jsonUser) ? null : JsonConvert.DeserializeObject<User>(jsonUser);
            }
            catch
            {
                return null;
            }
        }

        #region LogoutCommandImpl
        /// <summary>
        /// Implementation of the logout command.
        /// </summary>
        private class LogoutCommandImpl : ICommand
        {
            /// <inheritdoc />
            public event EventHandler CanExecuteChanged;

            /// <summary>
            /// Initializes a new instance of the <see cref="LogoutCommandImpl"/> class.
            /// </summary>
            internal LogoutCommandImpl()
            {
                UserChanged += (sender, args) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            /// <inheritdoc />
            public bool CanExecute(object parameter) => GetCurrentUser() != null;

            /// <inheritdoc />
            public async void Execute(object parameter)
            {
                //async void is ok (event handler)
                try
                {
                    await Factory.Get<ILogoutClient>().LogoutAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                UserChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
