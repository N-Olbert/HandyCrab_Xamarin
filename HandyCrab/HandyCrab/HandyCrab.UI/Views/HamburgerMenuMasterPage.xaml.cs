using System;
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

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage(), true);
        }
    }
}