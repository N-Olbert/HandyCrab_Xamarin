using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HandyCrab.Business.Fundamentals;
using HandyCrab.Business.Services;
using HandyCrab.Business.Services.BusinessObjects;
using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;
using JetBrains.Annotations;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandyCrab.Business.ViewModels
{
    internal class SearchViewModel : BaseViewModel, ISearchViewModel
    {
        private Command executeSearchCommand;
        private int selectedSearchRadiusBackingField;
        private Placemark currentPlacemarkBackingField;
        private Command setCurrentLocationCommand;
        private string selectedSearchOption;
        private bool searchWithPostcode;
        private string postcode;

        public event EventHandler SearchSucceeded;

        public IEnumerable<int> SearchRadiusInMeters => new[] {10, 25, 50, 100, 1000, 10000000};

        public int SelectedSearchRadius
        {
            get => this.selectedSearchRadiusBackingField;
            set
            {
                if (SearchRadiusInMeters.Contains(value))
                {
                    SetProperty(ref this.selectedSearchRadiusBackingField, value);
                }
            }
        }

        public Placemark CurrentPlacemark
        {
            get => this.currentPlacemarkBackingField;
            private set
            {
                Factory.Get<IInternalRuntimeDataStorageService>().StoreValue(StorageSlot.LastPlacemark, value);
                SetProperty(ref this.currentPlacemarkBackingField, value);
            }
        }

        public ICommand PerformSearchCommand { get; }

        public ICommand SetCurrentLocationCommand => this.setCurrentLocationCommand;

        public IEnumerable<string> SearchOptions => new string[] { "Positionsangabe", "Postleitzahl" };

        public string SelectedSearchOption
        { 
            get => this.selectedSearchOption;
            set
            {
                if (SearchOptions.Contains(value))
                {
                    if (value == "Positionsangabe")
                    {
                        SearchWithPostcode = false;
                    } else
                    {
                        SearchWithPostcode = true;
                        ApproximateCurrentGeolocationAsync();
                    }
                    SetProperty(ref this.selectedSearchOption, value);
                }
            }
        }

        public bool SearchWithPostcode
        {
            get => this.searchWithPostcode;
            private set
            {
                SetProperty(ref this.searchWithPostcode, value);
                this.executeSearchCommand.ChangeCanExecute();
            }
        }

        public string Postcode 
        { 
            get => this.postcode;
            set
            {
                SetProperty(ref this.postcode, value);
                this.executeSearchCommand.ChangeCanExecute();
            }
        }

        public SearchViewModel()
        {
            SelectedSearchRadius = SearchRadiusInMeters.ElementAt(1);
            this.executeSearchCommand = new Command(ExecuteSearch, CanExecuteSearch);
            PropertyChanged += (sender, args) => this.executeSearchCommand.ChangeCanExecute();
            PerformSearchCommand = this.executeSearchCommand;
            this.setCurrentLocationCommand = new Command(SetCurrentLocation);
            SelectedSearchOption = SearchOptions.First();

            var storageService = Factory.Get<IInternalRuntimeDataStorageService>();
            var currentPlacemark = storageService.GetValue<Placemark>(StorageSlot.SelectedManualPlacemark);
            if (currentPlacemark != null)
            {
                CurrentPlacemark = currentPlacemark;
                storageService.StoreValue(StorageSlot.SelectedManualPlacemark, null);
            }
            else
            {
                ApproximateCurrentGeolocationAsync();
            }
        }
        
        public async Task UpdateCurrentGeolocationAsync()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    await UpdateExactGeoPosition();
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async Task UpdateExactGeoPosition()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            var locationTask = Factory.Get<IGeolocationService>().GetLocationAsync(request);
            await UpdatePlacemarkAsync(locationTask);
        }

        private async void ExecuteSearch()
        {
            Failable<IEnumerable<Barrier>> barriers;
            var storage = Factory.Get<IInternalRuntimeDataStorageService>();
            if (SearchWithPostcode == false)
            {
                //async void is ok here (event handler)
                var current = CurrentPlacemark;
                barriers = await Factory.Get<IBarrierClient>()
                                            .GetBarriersAsync(current.Location.Longitude, current.Location.Latitude, SelectedSearchRadius);
            } else
            {
                barriers = await Factory.Get<IBarrierClient>()
                                            .GetBarriersAsync(Postcode);
            }
            if (barriers.IsSucceeded())
            {
                var barriersList = barriers.Value.ToList();
                for (int i = 0; i < barriersList.Count; i++)
                {
                    var distanceInKm = CurrentPlacemark.Location.CalculateDistance(barriersList[i].Latitude, 
                        barriersList[i].Longitude, DistanceUnits.Kilometers);
                    barriersList[i].DistanceToLocation = (int)(distanceInKm * 1000);
                }
                storage.StoreValue(StorageSlot.BarrierSearchPlacemark, CurrentPlacemark);
                storage.StoreValue(StorageSlot.BarrierSearchRadius, SelectedSearchRadius);
                storage.StoreValue(StorageSlot.BarrierSearchResults, barriers.Value);
                SearchSucceeded?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                storage.StoreValue(StorageSlot.BarrierSearchResults, null);
                RaiseOnError(barriers);
            }
        }

        private bool CanExecuteSearch()
        {
            return !IsBusy && ((SearchRadiusInMeters.Contains(SelectedSearchRadius) && CurrentPlacemark != null && SearchWithPostcode == false) ||
                (!String.IsNullOrEmpty(Postcode) && SearchWithPostcode == true));
        }

        private async Task ApproximateCurrentGeolocationAsync()
        {
            //Fast approximation
            var locationTask = Factory.Get<IGeolocationService>().GetLastKnownLocationAsync();
            await UpdatePlacemarkAsync(locationTask);

            //very accurate, but takes time
            UpdateExactGeoPosition();
        }

        private async Task UpdatePlacemarkAsync([NotNull]Task<Location> locationTask)
        {
            try
            {
                var loc = await locationTask;
                if (loc != null)
                {
                    IEnumerable<Placemark> placemarks = null;
                    try
                    {
                        placemarks = await Factory.Get<IGeolocationService>().GetPlacemarksAsync(loc);
                    }
                    catch
                    {
                        //Probably not cached and no connection or server down.
                        //https://issuetracker.google.com/issues/64247769
                        //https://stackoverflow.com/questions/16258898/does-android-geocoder-work-only-with-internet-connection
                    }

                    var placemark = placemarks?.FirstOrDefault();
                    CurrentPlacemark = placemark ?? new Placemark { Location = loc };
                }
            }
            catch (Exception e)
            {
                RaiseOnError(e);
            }
        }

        private void SetCurrentLocation()
        {
            ApproximateCurrentGeolocationAsync();
        }
    }
}