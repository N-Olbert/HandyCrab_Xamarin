using System;
using System.Windows.Input;

namespace HandyCrab.Common.Interfaces
{
    public interface ILoginViewModel : IViewModel
    {
        event EventHandler LoginSucceeded;

        event EventHandler<string> LoginRejected;
        
        string UserName { get; set; }

        string Password { get; set; }

        ICommand LoginCommand { get; }
    }
}