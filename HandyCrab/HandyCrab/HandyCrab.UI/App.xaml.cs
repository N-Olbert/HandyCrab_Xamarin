using HandyCrab.Views;
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
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            StartupHelper.Prepare();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
