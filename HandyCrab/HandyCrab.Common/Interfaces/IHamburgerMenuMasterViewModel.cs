using System;
using System.ComponentModel;
using System.Windows.Input;

namespace HandyCrab.Common.Interfaces
{
    public interface IHamburgerMenuMasterViewModel : IViewModel
    {
        event EventHandler OnLoginStatusChanged;
    }
}
