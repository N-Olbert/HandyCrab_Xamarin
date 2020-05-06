using System;
using System.Linq;
using System.Windows.Input;
using HandyCrab.Business.Fundamentals;
using HandyCrab.Business.Services;
using HandyCrab.Business.Services.BusinessObjects;
using HandyCrab.Common.Interfaces;
using JetBrains.Annotations;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.UI;
using Mapsui.Utilities;
using Mapsui.Widgets;
using Mapsui.Widgets.ScaleBar;
using Xamarin.Essentials;
using Xamarin.Forms;
using Map = Mapsui.Map;
using Point = Mapsui.Geometries.Point;

namespace HandyCrab.Business.ViewModels
{
    internal class LocationSelectionViewModel : BaseViewModel, ILocationSelectionViewModel
    {
        [NotNull]
        private readonly Feature geoPinFeature = new Feature {Geometry = new Point()};

        private Placemark selectedLocation;

        /// <inheritdoc />
        public event EventHandler RefreshMap;

        /// <inheritdoc />
        public event EventHandler OnConfirm;

        /// <inheritdoc />
        public event EventHandler OnCancel;

        /// <inheritdoc />
        public ICommand ConfirmCommand { get; }

        /// <inheritdoc />
        public ICommand CancelCommand { get; }

        /// <inheritdoc />
        public Map Map { get; }

        /// <inheritdoc />
        public Placemark SelectedLocation
        {
            get => this.selectedLocation;
            private set { SetProperty(ref this.selectedLocation, value); }
        }

        public LocationSelectionViewModel()
        {
            var map = Map = new Map();
            map.Layers.Add(OpenStreetMap.CreateTileLayer());
            map.Widgets.Add(new ScaleBarWidget(map)
            {
                TextAlignment = Alignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            });

            map.Layers.Add(CreatePinLayer(this.geoPinFeature));
            ConfirmCommand = new Command(() =>
            {
                Factory.Get<IInternalRuntimeDataStorageService>()
                                                    .StoreValue(StorageSlot.SelectedManualPlacemark, SelectedLocation);
                OnConfirm?.Invoke(this, EventArgs.Empty);
            });
            CancelCommand = new Command(() =>
            {
                Factory.Get<IInternalRuntimeDataStorageService>()
                                                   .StoreValue(StorageSlot.SelectedManualPlacemark, null);
                OnCancel?.Invoke(this, EventArgs.Empty);
            });
        }

        /// <inheritdoc />
        public async void MapViewOnInfo(object sender, MapInfoEventArgs e)
        {
            if (e != null && e.MapInfo != null && e.MapInfo.WorldPosition != null)
            {
                var worldPosition = e.MapInfo.WorldPosition;
                this.geoPinFeature.Geometry = worldPosition;
                this.geoPinFeature.RenderedGeometry?.Clear();
                RefreshMap?.Invoke(this, e);

                var lonLat = SphericalMercator.ToLonLat(worldPosition.X, worldPosition.Y);
                try
                {
                    var placemarks = await Factory.Get<IGeolocationService>().GetPlacemarksAsync(new Location(lonLat.Y, lonLat.X));
                    if (placemarks != null && placemarks.Any())
                    {
                        SelectedLocation = placemarks.FirstOrDefault();
                        var storageService = Factory.Get<IInternalRuntimeDataStorageService>();
                        storageService.StoreValue(StorageSlot.LastPlacemark, SelectedLocation);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }

        /// <inheritdoc />
        public override void OnViewShown(object sender, EventArgs e)
        {
            UpdateRegion();
            Map.Home = n =>
            {
                if (n != null && SelectedLocation != null)
                {
                    var location = SelectedLocation.Location;
                    if (location != null)
                    {
                        var point = SphericalMercator.FromLonLat(location.Longitude, location.Latitude);
                        this.geoPinFeature.Geometry = point;
                        this.geoPinFeature.RenderedGeometry?.Clear();
                        n.NavigateTo(point, Map.Resolutions.LastOrDefault());
                    }
                }
            };
        }

        private static MemoryLayer CreatePinLayer(Feature geoPinFeature)
        {
            return new MemoryLayer
            {
                IsMapInfoLayer = true,
                DataSource = new MemoryProvider(new Features{geoPinFeature}),
                Style = CreateBitmapStyle()
            };
        }
        
        private static SymbolStyle CreateBitmapStyle()
        {
            var path = "HandyCrab.Business.Resources.Map-Pin.png"; //Public domain: https://publicdomainvectors.org/de/kostenlose-vektorgrafiken/PIN-Karte/50742.html
            var assembly = typeof(LocationSelectionViewModel).Assembly;
            var image = assembly.GetManifestResourceStream(path);
            var bitmapId = BitmapRegistry.Instance.Register(image);
            var bitmapHeight = 116;
            return new SymbolStyle { BitmapId = bitmapId, SymbolScale = 0.6, SymbolOffset = new Offset(0, bitmapHeight * 0.5) };
        }

        private void UpdateRegion()
        {
            var storageService = Factory.Get<IInternalRuntimeDataStorageService>();
            var currentPlacemark = storageService.GetValue<Placemark>(StorageSlot.LastPlacemark);
            if (currentPlacemark != null)
            {
                SelectedLocation = currentPlacemark;
            }
        }
    }
}