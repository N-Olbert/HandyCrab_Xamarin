using System.Collections.Generic;
using HandyCrab.Common.Entitys;
using Xamarin.Essentials;

namespace HandyCrab.Common.Interfaces
{
    public interface ISearchResultsViewModel : IViewModel
    { 
        int SelectedSearchRadius { get; }

        Placemark CurrentPlacemark { get; }

        IEnumerable<IReadOnlyBarrier> SearchResults { get; }
    }
}