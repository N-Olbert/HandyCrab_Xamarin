using System;
using System.Windows.Input;
using HandyCrab.Common.Entitys;
using JetBrains.Annotations;

namespace HandyCrab.Common.Interfaces
{
    public interface ILoginViewModel : IViewModel
    {
        event EventHandler LoginSucceeded;

        event EventHandler<Failable> LoginRejected;

        string UserName { get; set; }

        [NotNull]
        string UserNameValidationRegex { get; }

        bool IsUserNameValid { get; }

        string Password { get; set; }

        [NotNull]
        string PasswordValidationRegex { get; }

        bool IsPasswordValid { get; }

        ICommand LoginCommand { get; }
    }
}