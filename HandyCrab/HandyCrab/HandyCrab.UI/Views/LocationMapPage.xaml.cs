using HandyCrab.Common.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyCrab.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationMapPage : ContentPage
    {
        public LocationMapPage()
        {
            InitializeComponent();
            var vm = (ILocationSelectionViewModel) BindingContext;
            this.mapView.Map = vm.Map; //must be set in code behind, not bindable

            this.Appearing += delegate (object sender, EventArgs args)
            {
                vm.RefreshMap += OnVmRefreshMapRequest;
                this.mapView.Info += vm.MapViewOnInfo;
                vm.OnViewShown(sender, args);
            };
            this.Disappearing += delegate (object sender, EventArgs args)
            {
                vm.RefreshMap -= OnVmRefreshMapRequest;
                this.mapView.Info -= vm.MapViewOnInfo;
                vm.OnViewHidden(sender, args);
            };

            vm.OnConfirm += (sender, args) => NavigationHelper.GoTo(new SearchPage());
            vm.OnCancel += (sender, args) => NavigationHelper.GoTo(new SearchPage());
        }

        private void OnVmRefreshMapRequest(object sender, EventArgs args)
        {
            this.mapView?.Refresh();
        }
    }
}