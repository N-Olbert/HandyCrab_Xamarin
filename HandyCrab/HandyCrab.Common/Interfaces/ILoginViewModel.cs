using System;
using System.Windows.Input;

namespace HandyCrab.Common.Interfaces
{
    public interface ILoginViewModel : IViewModel
    {
        event EventHandler LoginSucceeded;

        event EventHandler<Exception> LoginRejected;

        string UserName { get; set; }

        string UserNameValidationRegex { get; }

        bool IsUserNameValid { get; }

        string Password { get; set; }

        string PasswordValidationRegex { get; }

        bool IsPasswordValid { get; }

        ICommand LoginCommand { get; }
    }
}