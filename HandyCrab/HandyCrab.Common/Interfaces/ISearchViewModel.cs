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

        [NotNull]
        ICommand PerformSearchCommand { get; }

        [NotNull]
        Task UpdateCurrentGeolocationAsync();
    }
}