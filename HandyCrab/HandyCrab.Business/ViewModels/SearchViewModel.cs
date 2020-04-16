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

        public event EventHandler SearchSucceeded;

        public IEnumerable<int> SearchRadiusInMeters => new[] {5, 15, 30};

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
            private set => SetProperty(ref this.currentPlacemarkBackingField, value);
        }

        public ICommand PerformSearchCommand { get; }

        public SearchViewModel()
        {
            ApproximateCurrentGeolocationAsync();
            SelectedSearchRadius = SearchRadiusInMeters.Last();
            this.executeSearchCommand = new Command(ExecuteSearch, CanExecuteSearch);
            PropertyChanged += (sender, args) => this.executeSearchCommand.ChangeCanExecute();

            PerformSearchCommand = this.executeSearchCommand;
        }
        
        public async Task UpdateCurrentGeolocationAsync()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    var locationTask = Factory.Get<IGeolocationService>().GetLocationAsync(request);
                    await UpdatePlacemarkAsync(locationTask);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private void ExecuteSearch()
        {
            //Fake values (until backend supports it)
            var storage = Factory.Get<IInternalRuntimeDataStorageService>();
            storage.StoreValue(StorageSlot.BarrierSearchPlacemark, CurrentPlacemark);
            storage.StoreValue(StorageSlot.BarrierSearchRadius, SelectedSearchRadius);
            storage.StoreValue(StorageSlot.BarrierSearchResults, new []
            {
                new Barrier { Description = "Test Desc 1", Title = "Barrier 1", Postcode = "70000"},
                new Barrier { Description = "Test Desc 2", Title = "Barrier 2", Postcode = "70000", 
                    Picture = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Adeliepinguine-Landgang.jpg/1024px-Adeliepinguine-Landgang.jpg"}
            });

            SearchSucceeded?.Invoke(this, EventArgs.Empty);
        }

        private bool CanExecuteSearch()
        {
            return !IsBusy && SearchRadiusInMeters.Contains(SelectedSearchRadius) && CurrentPlacemark != null;
        }

        private async Task ApproximateCurrentGeolocationAsync()
        {
            //Fast approximation
            var locationTask = Factory.Get<IGeolocationService>().GetLastKnownLocationAsync();
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
                //TODO: Implement Exception handling
            }
        }
    }
}