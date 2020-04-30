using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using HandyCrab.Business.Fundamentals;
using HandyCrab.Business.Services;
using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;
using JetBrains.Annotations;
using Xamarin.Essentials;
using Xamarin.Forms;
using Barrier = HandyCrab.Common.Entitys.Barrier;
using Exception = System.Exception;

namespace HandyCrab.Business.ViewModels
{
    internal class AddOrModifyBarrierViewModel : BaseViewModel, IAddOrModifyBarrierViewModel
    {
        private string modifiedBarrierId;
        private string title;
        private double longitude;
        private double latitude;
        private string description;
        private string postcode;
        private string initialSolutionText;
        private ImageSource image;
        [NotNull]
        private readonly Command addOrModifyBarrierCommand;

        /// <inheritdoc />
        public event EventHandler<IReadOnlyBarrier> OnSuccess; 

        /// <inheritdoc />
        public string ModifiedBarrierId
        {
            get => this.modifiedBarrierId;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var storageService = Factory.Get<IInternalRuntimeDataStorageService>();
                    var barriers = storageService.GetValue<IEnumerable<Barrier>>(StorageSlot.BarrierSearchResults);
                    var barrierToModify = barriers?.FirstOrDefault(x => x != null && x.Id == value);
                    if (barrierToModify != null)
                    {
                        SetProperty(ref this.modifiedBarrierId, value);
                        Title = barrierToModify.Title;
                        Longitude = barrierToModify.Longitude;
                        Latitude = barrierToModify.Latitude;
                        Description = barrierToModify.Description;
                        Postcode = barrierToModify.Postcode;
                        InitialSolutionText = null;
                        var imageSource = new UriImageSource();
                        if (!string.IsNullOrEmpty(barrierToModify.Picture))
                        {
                            imageSource.Uri = new Uri(barrierToModify.Picture);
                        }

                        Image = imageSource;
                    }
                    else
                    {
                        RaiseOnError(new InvalidOperationException("Requested barrier does not exist."));
                    }
                }
                else
                {
                    Title = string.Empty;
                    Longitude = 0;
                    Latitude = 0;
                    Description = string.Empty;
                    Postcode = string.Empty;
                    InitialSolutionText = string.Empty;
                    Image = new StreamImageSource();
                }

                this.addOrModifyBarrierCommand.ChangeCanExecute();
            }
        }

        /// <inheritdoc />
        public string Title
        {
            get => this.title;
            set
            {
                SetProperty(ref this.title, value);
                this.addOrModifyBarrierCommand.ChangeCanExecute();
            }
        }

        /// <inheritdoc />
        public double Longitude
        {
            get => this.longitude;
            set
            {
                SetProperty(ref this.longitude, value);
                this.addOrModifyBarrierCommand.ChangeCanExecute();
            }
        }

        /// <inheritdoc />
        public double Latitude
        {
            get => this.latitude;
            set
            {
                SetProperty(ref this.latitude, value);
                this.addOrModifyBarrierCommand.ChangeCanExecute();
            }
        }

        /// <inheritdoc />
        public string Description
        {
            get => this.description;
            set
            {
                SetProperty(ref this.description, value);
                this.addOrModifyBarrierCommand.ChangeCanExecute();
            }
        }

        /// <inheritdoc />
        public string Postcode
        {
            get => this.postcode;
            set
            {
                SetProperty(ref this.postcode, value);
                this.addOrModifyBarrierCommand.ChangeCanExecute();
            }
        }

        /// <inheritdoc />
        public string InitialSolutionText
        {
            get => this.initialSolutionText;
            set
            {
                SetProperty(ref this.initialSolutionText, value);
                this.addOrModifyBarrierCommand.ChangeCanExecute();
            }
        }

        /// <inheritdoc />
        public ImageSource Image
        {
            get => this.image;
            set => SetProperty(ref this.image, value);
        }

        /// <inheritdoc />
        public ICommand AddOrModifyBarrierCommand
        {
            get => this.addOrModifyBarrierCommand;
        }

        public AddOrModifyBarrierViewModel()
        {
            this.addOrModifyBarrierCommand = new Command(AddOrModifyBarrier, CanExecuteAddOrModifyCommand);
            var storageService = Factory.Get<IInternalRuntimeDataStorageService>();
            var placemark = storageService.GetValue<Placemark>(StorageSlot.BarrierSearchPlacemark);
            if (placemark != null)
            {
                if (placemark.Location != null)
                {
                    Latitude = placemark.Location.Latitude;
                    Longitude = placemark.Location.Longitude;
                }

                Postcode = placemark.PostalCode;
            }
        }

        private bool CanExecuteAddOrModifyCommand()
        {
            if (!string.IsNullOrEmpty(ModifiedBarrierId))
            {
                return !string.IsNullOrEmpty(Title);
            }

            return !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Postcode) && Longitude != 0 && Latitude != 0;
        }

        private async void AddOrModifyBarrier()
        {
            try
            {

            }
            catch (Exception e)
            {
                RaiseOnError(e);
            }
            //async void is ok here (event handler)
            var client = Factory.Get<IBarrierClient>();
            string picture = null;
            if (Image is StreamImageSource s)
            {
                picture = ConvertToBase64(await s.Stream(CancellationToken.None));
            }

            var storageService = Factory.Get<IInternalRuntimeDataStorageService>();
            var barriers = storageService.GetValue<System.Collections.Generic.IList<Barrier>>(StorageSlot.BarrierSearchResults);
            if (!string.IsNullOrEmpty(ModifiedBarrierId))
            {
                var barrierToModify = barriers?.FirstOrDefault(x => x != null && x.Id == ModifiedBarrierId);
                var updatedBarrier = await client.ModifyBarrierAsync(ModifiedBarrierId, Title, picture, Description);
                if (updatedBarrier.IsSucceeded())
                {
                    barriers.Remove(barrierToModify);
                    barriers.Add(updatedBarrier.Value);
                    OnSuccess?.Invoke(this, updatedBarrier.Value);
                }
                else
                {
                    RaiseOnError(updatedBarrier);
                }
            }
            else
            {
                var temp = new Barrier
                {
                    Description = Description, Latitude = Latitude, Longitude = Longitude, Title = Title,
                    Postcode = Postcode
                };
                var initialSolution = new Solution {Text = InitialSolutionText};


                var newBarrier = await client.AddBarrierAsync(temp, picture, initialSolution);
                if (newBarrier.IsSucceeded())
                {
                    barriers.Add(newBarrier.Value);
                    OnSuccess?.Invoke(this, newBarrier.Value);
                }
                else
                {
                    RaiseOnError(newBarrier);
                }
            }
        }

        public static string ConvertToBase64(Stream stream)
        {
            if (stream != null)
            {
                byte[] bytes;
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                return Convert.ToBase64String(bytes);
            }

            return null;
        }
    }
}