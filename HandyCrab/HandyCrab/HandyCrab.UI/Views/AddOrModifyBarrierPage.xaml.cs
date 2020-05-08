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
        public AddOrModifyBarrierPage(string modifiedBarrierId = "")
        {
            InitializeComponent();

            var vm = (IAddOrModifyBarrierViewModel)BindingContext;

            if (!String.IsNullOrEmpty(modifiedBarrierId))
            {
                vm.ModifiedBarrierId = modifiedBarrierId;
            }

            vm.OnSuccess += async (sender, args) =>
            {
                await Navigation.PopToRootAsync();
            };

            vm.OnError += (sender, args) => OnError(sender, args);
        }

        private void OnError(object sender, Failable e)
        {
            switch (e.ErrorCode)
            {
                case 1:
                    DisplayAlert("Fehler", Strings.Error_UnknownError, "OK");
                    break;
                case 2:
                    DisplayAlert("Fehler", Strings.Error_NotLoggedIn, "OK");
                    break;
                case 14:
                    DisplayAlert("Fehler", Strings.Error_PictureTooBig, "OK");
                    break;
                case 15:
                    DisplayAlert("Fehler", Strings.Error_InvalidPictureFormat, "OK");
                    break;
                case 2147483647:
                    DisplayAlert("Fehler", Strings.Error_NetworkTimeout, "OK");
                    break;
                default:
                    DisplayAlert("Fehler", Strings.Error_UnknownError, "OK");
                    break;
            }
        }
    }
}