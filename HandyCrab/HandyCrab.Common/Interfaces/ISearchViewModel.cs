using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using Xamarin.Essentials;

namespace HandyCrab.Common.Interfaces
{
    public interface ISearchViewModel : IViewModel
    {
        event EventHandler SearchSucceeded;

        [NotNull]
        IEnumerable<int> SearchRadiusInMeters { get; }

        int SelectedSearchRadius { get; set; }

        Placemark CurrentPlacemark { get; }

        IEnumerable<string> SearchOptions { get; }

        string SelectedSearchOption { get; set; }

        bool SearchWithPostcode { get; }

        string Postcode { get; set; }

        [NotNull]
        ICommand PerformSearchCommand { get; }

        ICommand SetCurrentLocationCommand { get; }

        [NotNull]
        Task UpdateCurrentGeolocationAsync();
    }
}