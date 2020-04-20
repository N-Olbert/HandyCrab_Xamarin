using System;
using System.ComponentModel;
using HandyCrab.Common.Entitys;

namespace HandyCrab.Common.Interfaces
{
    /// <summary>
    /// Base interface of any view model
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Occurs when an error occurs.
        /// </summary>
        event EventHandler<Failable> OnError;

        /// <summary>
        /// Gets the current user. May be null if no user logged in.
        /// </summary>
        User CurrentUser { get; }
    }
}