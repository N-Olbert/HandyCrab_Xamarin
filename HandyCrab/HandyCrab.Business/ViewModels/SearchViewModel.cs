﻿using System;
using System.Linq;
using System.Threading.Tasks;
using HandyCrab.Common.Interfaces;
using JetBrains.Annotations;
using Xamarin.Essentials;

namespace HandyCrab.Business.ViewModels
{
    internal class SearchViewModel : BaseViewModel, ISearchViewModel
    {
        public override string PageTitle => "Search";

        private Placemark currentPlacemarkBackingField;
        public Placemark CurrentPlacemark
        {
            get => this.currentPlacemarkBackingField;
            private set => SetProperty(ref this.currentPlacemarkBackingField, value);
        }

        public SearchViewModel()
        {
            ApproximateCurrentGeolocationAsync();
        }

        public async Task UpdateCurrentGeolocationAsync()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    var locationTask = Geolocation.GetLocationAsync(request);
                    await UpdatePlacemarkAsync(locationTask);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async Task ApproximateCurrentGeolocationAsync()
        {
            //Fast approximation
            var locationTask = Geolocation.GetLastKnownLocationAsync();
            await UpdatePlacemarkAsync(locationTask);

            //very accurate, but takes time
            UpdateCurrentGeolocationAsync(); 
        }

        private async Task UpdatePlacemarkAsync([NotNull]Task<Location> locationTask)
        {
            try
            {
                var loc = await locationTask;
                if (loc != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(loc);

                    var placemark = placemarks?.FirstOrDefault();
                    CurrentPlacemark = placemark ?? new Placemark { Location = loc };
                }
            }
            catch (Exception e)
            {
                //TODO: Implement Exception handling
            }
        }
    }
}