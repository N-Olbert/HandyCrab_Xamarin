using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using HandyCrab.Common.Interfaces;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrModifyBarrierPage : ContentPage
    {
        public AddOrModifyBarrierPage(string modifiedBarrierId = "")
        {
            InitializeComponent();

            var vm = (IAddOrModifyBarrierViewModel) BindingContext;

            if (!String.IsNullOrEmpty(modifiedBarrierId))
            {
                vm.ModifiedBarrierId = modifiedBarrierId;
            }

            vm.OnSuccess += async (sender, args) =>
            {
                await Navigation.PopToRootAsync();
            };
        }
    }
}