using System.ComponentModel;
using Xamarin.Forms;
using HandyCrab.Common.Interfaces;

namespace HandyCrab.UI.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            Title = "Super";
            InitializeComponent();
            var vm = (IMainViewModel)BindingContext;
            if (vm.CurrentUser == null)
            {
                this.Detail = new NavigationPage(new LoginPage());
            } else
            {
                this.Detail = new NavigationPage(new SearchPage());
            }
        }
    }
}
