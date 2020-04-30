﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultsPage : ContentPage
    {
        public SearchResultsPage()
        {
            InitializeComponent();
        }

        void OnAddBarrierButtonClicked(object sender, EventArgs args)
        {
            NavigationHelper.GoTo(new AddOrModifyBarrierPage());
        }
    }
}