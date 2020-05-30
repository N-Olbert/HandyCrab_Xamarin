using System;
using Xamarin.Forms.Xaml;

using HandyCrab.Common.Interfaces;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarrierPage : BaseContentPage
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
            vm.OnError += OnError;
        }

        void OnAddSolutionButtonClicked(object sender, EventArgs args)
        {
            addSolutionButton.IsVisible = false;
            addSolutionTextField.IsVisible = true;
            confirmAddSolutionButton.IsVisible = true;
        }
    }
}