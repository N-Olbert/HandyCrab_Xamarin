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

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            //TODO: send data to the backend for verification, only redirect to search page if successful
            MasterDetailPage MasterPage = App.Current.MainPage as MasterDetailPage;
            MasterPage.Detail = new NavigationPage(new SearchPage());
            MasterPage.IsPresented = false;
        }
    }
}