using System;
using System.Collections.Generic;
using System.Text;
using HandyCrab.Common.Interfaces;

namespace HandyCrab.Business.ViewModels
{
    class HamburgerMenuMasterViewModel : BaseViewModel, IHamburgerMenuMasterViewModel
    {
        public event EventHandler OnLoginStatusChanged;

        public HamburgerMenuMasterViewModel()
        {
            UserChanged += (sender, args) => OnLoginStatusChanged.Invoke(sender, args);
        }
    }
}
