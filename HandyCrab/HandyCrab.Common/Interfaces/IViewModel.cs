using System;
using System.ComponentModel;

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
        event EventHandler<string> OnError;
    }
}