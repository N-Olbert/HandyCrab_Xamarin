using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HandyCrab.Common.Interfaces
{
    public interface ISearchViewModel : IViewModel
    {
        Placemark CurrentPlacemark { get; }
        Task UpdateCurrentGeolocationAsync();
    }
}