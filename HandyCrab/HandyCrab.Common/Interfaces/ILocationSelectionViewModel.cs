using System;
using System.Windows.Input;
using Mapsui.UI;
using Xamarin.Essentials;
using Map = Mapsui.Map;

namespace HandyCrab.Common.Interfaces
{
    public interface ILocationSelectionViewModel : IViewModel
    {
        event EventHandler RefreshMap;

        event EventHandler OnConfirm;

        event EventHandler OnCancel;

        ICommand ConfirmCommand { get; }

        ICommand CancelCommand { get; }

        Map Map { get; }

        Placemark SelectedLocation { get; }

        void MapViewOnInfo(object sender, MapInfoEventArgs e);
    }
}