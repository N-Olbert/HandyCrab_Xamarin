using System;
using System.Windows.Input;
using HandyCrab.Common.Entitys;
using JetBrains.Annotations;

namespace HandyCrab.Common.Interfaces
{
    public interface IRegisterViewModel : IViewModel
    {
        event EventHandler RegisterSucceeded;

        event EventHandler<Failable> RegisterRejected;

        string UserName { get; set; }

        [NotNull]
        string UserNameValidationRegex { get; }

        bool IsUserNameValid { get; }

        string Password { get; set; }

        [NotNull]
        string PasswordValidationRegex { get; }

        bool IsPasswordValid { get; }

        string Email { get; set; }

        [NotNull]
        string EmailValidationRegex { get; }

        bool IsEmailValid { get; }

        ICommand RegisterCommand { get; }
    }
}