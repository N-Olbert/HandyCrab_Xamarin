using System.Collections.Generic;
using System.Linq;
using HandyCrab.Business.Fundamentals;
using HandyCrab.Business.Services;
using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;
using Xamarin.Essentials;

namespace HandyCrab.Business.ViewModels
{
    internal class SearchResultsViewModel : BaseViewModel, ISearchResultsViewModel
    {
        private int selectedSearchRadius;
        private Placemark currentPlacemark;
        private IEnumerable<IReadOnlyBarrier> searchResults;

        public int SelectedSearchRadius
        {
            get => this.selectedSearchRadius;
            set => SetProperty(ref this.selectedSearchRadius, value);
        }

        public Placemark CurrentPlacemark
        {
            get => this.currentPlacemark;
            set => SetProperty(ref this.currentPlacemark, value);
        }

        public IEnumerable<IReadOnlyBarrier> SearchResults
        {
            get => this.searchResults;
            set => SetProperty(ref this.searchResults, value?.ToList());
        }

        public SearchResultsViewModel()
        {
            UpdateSearchResults();
            InternalRuntimeDataStorageService.StorageValueChanged += OnStorageValueChanged;
        }

        private void OnStorageValueChanged(object sender, StorageSlot slot)
        {
            switch (slot)
            {
                case StorageSlot.BarrierSearchRadius:
                case StorageSlot.BarrierSearchResults:
                case StorageSlot.BarrierSearchPlacemark:
                    UpdateSearchResults();
                    break;
            }
        }

        private void UpdateSearchResults()
        {
            var storage = Factory.Get<IInternalRuntimeDataStorageService>();
            SelectedSearchRadius = storage.GetValue<int>(StorageSlot.BarrierSearchRadius);
            SearchResults = storage.GetValue<IEnumerable<Barrier>>(StorageSlot.BarrierSearchResults);
            CurrentPlacemark = storage.GetValue<Placemark>(StorageSlot.BarrierSearchPlacemark);
        }
    }
}