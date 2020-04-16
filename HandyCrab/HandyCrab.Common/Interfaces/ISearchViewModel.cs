using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace HandyCrab.Common.Interfaces
{
    public interface ISearchViewModel : IViewModel
    {
        event EventHandler SearchSucceeded;

        IEnumerable<int> SearchRadiusInMeters { get; }

        int SelectedSearchRadius { get; set; }

        Placemark CurrentPlacemark { get; }

        ICommand PerformSearchCommand { get; }

        Task UpdateCurrentGeolocationAsync();
    }
}