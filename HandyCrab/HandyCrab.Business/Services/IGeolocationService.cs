using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public interface IGeolocationService
    {
        Task<Location> GetLastKnownLocationAsync();

        Task<Location> GetLocationAsync(GeolocationRequest request);

        Task<IEnumerable<Placemark>> GetPlacemarksAsync(Location location);
    }
}