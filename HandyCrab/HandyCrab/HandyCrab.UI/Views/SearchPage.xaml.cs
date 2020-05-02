using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCrab.Common;
using HandyCrab.Common.Interfaces;
using HandyCrab.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
            var vm = (ISearchViewModel)BindingContext;

            vm.SearchSucceeded += (sender, args) => Navigation.PushAsync(new SearchResultsPage());

            IdToBoolConverter.userId = vm.CurrentUser.Id;
        }
    }
}