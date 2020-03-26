﻿using HandyCrab.Business.ViewModels;
using HandyCrab.Common;
using HandyCrab.Common.Interfaces;

namespace HandyCrab.Business
{
    public static class StartupHelper
    {
        public static void Prepare()
        {
            ViewModelFactory.RegisterInstance<IAboutViewModel, AboutViewModel>();
        }
    }
}