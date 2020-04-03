﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCrab.Common;
using HandyCrab.Common.Interfaces;
using HandyCrab.UI.Views; //FIXME: adjust namespace
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        async private void SearchButton_Clicked(object sender, EventArgs e)
        {
            //TODO: send search Request to Backend and return Results
            await Navigation.PushAsync(new SearchResultsPage());
        }
    }
}