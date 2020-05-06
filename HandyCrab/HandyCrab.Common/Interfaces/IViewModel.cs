using System;
using System.ComponentModel;
using System.Windows.Input;
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
        /// Called when the corresponding view is shown.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void OnViewShown(object sender, EventArgs e);

        /// <summary>
        /// Called when the corresponding view is hidden.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void OnViewHidden(object sender, EventArgs e);

        /// <summary>
        /// Gets the current user. May be null if no user logged in.
        /// </summary>
        User CurrentUser { get; }

        /// <summary>
        /// Gets the logout command.
        /// </summary>
        ICommand LogoutCommand { get; }
    }
}