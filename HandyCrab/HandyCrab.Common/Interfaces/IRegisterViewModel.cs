using System;
using System.Windows.Input;

namespace HandyCrab.Common.Interfaces
{
    public interface IRegisterViewModel : IViewModel
    {
        event EventHandler RegisterSucceeded;

        event EventHandler<string> RegisterRejected;

        string EmailPlaceholder { get; }

        string UserNamePlaceholder { get; }

        string PasswordPlaceholder { get; }

        string SignUpButtonText { get; }

        string UserName { get; set; }

        string Email { get; set; }

        string Password { get; set; }

        ICommand RegisterCommand { get; }
    }
}