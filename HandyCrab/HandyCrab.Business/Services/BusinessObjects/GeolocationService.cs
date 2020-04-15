using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HandyCrab.Business.Services.BusinessObjects
{
    internal class GeolocationService : IGeolocationService
    {
        public Task<Location> GetLastKnownLocationAsync()
        {
            return Geolocation.GetLastKnownLocationAsync();
        }

        public Task<Location> GetLocationAsync(GeolocationRequest request)
        {
            return Geolocation.GetLocationAsync(request);
        }

        public Task<IEnumerable<Placemark>> GetPlacemarksAsync(Location location)
        {
            return Geocoding.GetPlacemarksAsync(location);
        }
    }
}