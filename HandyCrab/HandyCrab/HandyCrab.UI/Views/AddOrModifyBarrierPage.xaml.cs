using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using HandyCrab.Common.Interfaces;
using HandyCrab.Common.Entitys;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrModifyBarrierPage : ContentPage
    {
        public AddOrModifyBarrierPage()
        {
            InitializeComponent();

            var vm = (IAddOrModifyBarrierViewModel) BindingContext;

            vm.OnSuccess += async (sender, args) =>
            {
                await Navigation.PopToRootAsync();
            };
        }
    }
}