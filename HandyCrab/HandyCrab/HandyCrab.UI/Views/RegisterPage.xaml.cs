using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            //Go to next page
            await Navigation.PushAsync(new SearchPage());
        }
    }
}