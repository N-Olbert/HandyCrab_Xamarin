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
    public partial class BarrierPage : ContentPage
    {
        public BarrierPage(string id)
        {
            InitializeComponent();

            var vm = (IBarrierViewModel)BindingContext;
            vm.BarrierId = id;
        }
    }
}