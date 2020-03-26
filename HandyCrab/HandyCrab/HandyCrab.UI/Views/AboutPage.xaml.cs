using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCrab.Common;
using HandyCrab.Common.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        private IAboutViewModel viewModel;
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = this.viewModel = ViewModelFactory.GetInstance<IAboutViewModel>();
        }
    }
}