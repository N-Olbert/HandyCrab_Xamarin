using HandyCrab.UI.Views;
using System;
using HandyCrab.Business;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab
{
    public partial class App : Application
    {
        public App()
        {
            StartupHelper.Prepare();
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
