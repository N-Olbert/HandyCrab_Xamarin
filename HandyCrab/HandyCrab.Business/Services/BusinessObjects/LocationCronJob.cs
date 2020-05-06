using System;
using System.Linq;
using System.Threading;
using HandyCrab.Business.Fundamentals;
using Xamarin.Essentials;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public class LocationCronJob
    {
        private int interval;
        private long running;
        private Timer timer;

        public LocationCronJob(int interval)
        {
            this.interval = interval;
            this.timer = new Timer(TimedProcess, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void Start()
        {
            Interlocked.Exchange(ref this.running, 1);
            this.timer.Change(0, Timeout.Infinite);
        }

        public void Stop()
        {
            Interlocked.Exchange(ref this.running, 0);
        }

        private async void TimedProcess(object state)
        {
            if (Interlocked.Read(ref this.running) == 1)
            {
                try
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Best);
                    var location = await Factory.Get<IGeolocationService>().GetLocationAsync(request);
                    var placemark = await Factory.Get<IGeolocationService>().GetPlacemarksAsync(location);
                    if (placemark != null && placemark.Any())
                    {
                        Factory.Get<IInternalRuntimeDataStorageService>()
                               .StoreValue(StorageSlot.LastPlacemark, placemark.FirstOrDefault());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            if (Interlocked.Read(ref this.running) == 1)
            {
                this.timer.Change(this.interval, Timeout.Infinite);
            }
        }
    }
}