﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCrab.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HamburgerMenuMasterPage : ContentPage
    {
        public HamburgerMenuMasterPage()
        {
            InitializeComponent();
            Title = "Hamburger";
        }

        private void LoginButton_OnClicked(object sender, EventArgs e)
        {
            MasterDetailPage MasterPage = App.Current.MainPage as MasterDetailPage;
            MasterPage.Detail = new NavigationPage(new LoginPage());
            MasterPage.IsPresented = false;
        }

        private void RegisterButton_OnClicked(object sender, EventArgs e)
        {
            MasterDetailPage MasterPage = App.Current.MainPage as MasterDetailPage;
            MasterPage.Detail = new NavigationPage(new RegisterPage());
            MasterPage.IsPresented = false;
        }

        private void AboutButton_OnClicked(object sender, EventArgs e)
        {
            MasterDetailPage MasterPage = App.Current.MainPage as MasterDetailPage;
            MasterPage.Detail = new NavigationPage(new AboutPage());
            MasterPage.IsPresented = false;
        }
    }
}