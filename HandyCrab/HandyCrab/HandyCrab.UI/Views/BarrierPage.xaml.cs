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
    public partial class BarrierPage : ContentPage
    {
        public BarrierPage(string id)
        {
            InitializeComponent();

            var vm = (IBarrierViewModel)BindingContext;
            vm.BarrierId = id;
            vm.AddSolutionSucceeded += (sender, args) =>
            {
                addSolutionTextField.IsVisible = false;
                confirmAddSolutionButton.IsVisible = false;
                addSolutionButton.IsVisible = true;
                vm.NewSolutionText = "";
            };
            vm.OnError += (sender, args) => OnError(sender, args);
        }

        void OnAddSolutionButtonClicked(object sender, EventArgs args)
        {
            addSolutionButton.IsVisible = false;
            addSolutionTextField.IsVisible = true;
            confirmAddSolutionButton.IsVisible = true;
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
                case 9:
                    DisplayAlert("Fehler", Strings.Error_BarrierNotFound, "OK");
                    break;
                case 10:
                    DisplayAlert("Fehler", Strings.Error_NotLoggedIn, "OK");
                    break;
                case 11:
                    DisplayAlert("Fehler", Strings.Error_SolutionNotFound, "OK");
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