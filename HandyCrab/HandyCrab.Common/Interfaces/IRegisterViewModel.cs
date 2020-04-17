using System;
using System.Windows.Input;

namespace HandyCrab.Common.Interfaces
{
    public interface IRegisterViewModel : IViewModel
    {
        event EventHandler RegisterSucceeded;

        event EventHandler<Exception> RegisterRejected;

        string EmailPlaceholder { get; }

        string UserNamePlaceholder { get; }

        string PasswordPlaceholder { get; }

        string SignUpButtonText { get; }

        string UserName { get; set; }

        string UserNameValidationRegex { get; }

        bool IsUserNameValid { get; }

        string Password { get; set; }

        string PasswordValidationRegex { get; }

        bool IsPasswordValid { get; }

        string Email { get; set; }
        
        string EmailValidationRegex { get; }

        bool IsEmailValid { get; }

        ICommand RegisterCommand { get; }
    }
}