using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HandyCrab.Common.Interfaces;

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
