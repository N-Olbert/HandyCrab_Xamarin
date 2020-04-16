using System;
using System.Windows.Input;

namespace HandyCrab.Common.Interfaces
{
    public interface ILoginViewModel : IViewModel
    {
        event EventHandler LoginSucceeded;

        event EventHandler<Exception> LoginRejected;

        string UserNamePlaceholder { get; }

        string PasswordPlaceholder { get; }

        string LoginButtonText { get; }

        string NoAccountButtonText { get; }

        string UserName { get; set; }

        string Password { get; set; }

        ICommand LoginCommand { get; }
    }
}