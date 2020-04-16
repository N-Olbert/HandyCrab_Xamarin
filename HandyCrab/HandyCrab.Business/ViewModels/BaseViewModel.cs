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

        public bool IsBusy
        {
            get => this.isBusy;
            set => SetProperty(ref this.isBusy, value);
        }

        public event EventHandler<string> OnError;

        public User CurrentUser
        {
            get
            {
                var jsonUser = Factory.Get<ISecureStorage>().GetAsync(nameof(StorageSlot.CurrentUser))?.GetAwaiter().GetResult();
                return string.IsNullOrEmpty(jsonUser) ? null : JsonConvert.DeserializeObject<User>(jsonUser);
            }
        }

        protected void SetProperty<T>(ref T backingStore, T value,
                                      [CallerMemberName]string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                backingStore = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
