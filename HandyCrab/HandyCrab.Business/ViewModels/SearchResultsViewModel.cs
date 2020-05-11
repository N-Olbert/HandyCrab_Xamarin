using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using HandyCrab.Business.Fundamentals;
using HandyCrab.Business.Services;
using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandyCrab.Business.ViewModels
{
    internal class SearchResultsViewModel : BaseViewModel, ISearchResultsViewModel
    {
        private int selectedSearchRadius;
        private Placemark currentPlacemark;
        private IEnumerable<IReadOnlyBarrier> searchResults;
        private IEnumerable<string> sortOptions;
        private string selectedSearchOption;
        private Command deleteBarrierCommand;

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

        public IEnumerable<string> SortOptions => new[] { "Distanz", "Datum", "Alphabetisch" };

        public string SelectedSortOption
        {
            get => this.selectedSearchOption;
            set
            {
                if (SortOptions.Contains(value))
                {
                    SetProperty(ref this.selectedSearchOption, value);
                    switch(value)
                    {
                        case "Distanz":
                            SearchResults = SearchResults.OrderBy(x => x.DistanceToLocation);
                            break;
                        case "Datum":
                            SearchResults = SearchResults.OrderByDescending(x => x.Id);
                            break;
                        case "Alphabetisch":
                            SearchResults = SearchResults.OrderBy(x => x.Title);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public ICommand DeleteBarrierCommand => this.deleteBarrierCommand;

        public SearchResultsViewModel()
        {
            UpdateSearchResults();
            InternalRuntimeDataStorageService.StorageValueChanged += OnStorageValueChanged;
            SelectedSortOption = SortOptions.First();
            this.deleteBarrierCommand = new Command<string>(deleteBarrierAction);
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

        private async void deleteBarrierAction(string barrierId)
        {
            var client = Factory.Get<IBarrierClient>();
            var task = await client.DeleteBarrierAsync(barrierId);
            if (task.IsSucceeded())
            {
                var barriersList = SearchResults.ToList();
                for (int i = 0; i < barriersList.Count; i++)
                {
                    if (barriersList[i].Id == barrierId)
                    {
                        barriersList.RemoveAt(i);
                        SearchResults = barriersList;
                        break;
                    }
                }
            } else
            {
                RaiseOnError(task);
            }
        }
    }
}