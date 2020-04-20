using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HandyCrab.Business.Services;
using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;
using Newtonsoft.Json;

namespace HandyCrab.Business.ViewModels
{
    internal abstract class BaseViewModel : IViewModel
    {
        private bool isBusy;
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<Failable> OnError;

        public bool IsBusy
        {
            get => this.isBusy;
            set => SetProperty(ref this.isBusy, value);
        }

        public User CurrentUser
        {
            get
            {
                return GetCurrentUser();
            }
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

        public void RaiseOnError(Exception exception)
        {
            OnError?.Invoke(this, new Failable(exception));
        }

        private static User GetCurrentUser()
        {
            try
            {
                var jsonUser = Factory.Get<ISecureStorage>().GetAsync(nameof(StorageSlot.CurrentUser))?.GetAwaiter().GetResult();
                return string.IsNullOrEmpty(jsonUser) ? null : JsonConvert.DeserializeObject<User>(jsonUser);
            }
            catch
            {
                return null;
            }
        }
    }
}
