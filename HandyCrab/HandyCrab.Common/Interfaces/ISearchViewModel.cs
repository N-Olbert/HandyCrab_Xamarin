using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HandyCrab.Common.Interfaces
{
    public interface ISearchViewModel : IViewModel
    {
        string PageTitle { get; }

        Placemark CurrentPlacemark { get; }

        Task UpdateCurrentGeolocationAsync();
    }
}