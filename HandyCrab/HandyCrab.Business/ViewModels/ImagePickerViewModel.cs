using System;
using System.Windows.Input;
using HandyCrab.Business.Services;
using HandyCrab.Common.Interfaces;
using JetBrains.Annotations;
using Xamarin.Forms;

namespace HandyCrab.Business.ViewModels
{
    internal abstract class ImagePickerViewModel : BaseViewModel, IImagePickerViewModel
    {
        private readonly ImageCommand takeImageCommand;
        private readonly ImageCommand selectImageCommand;
        private ImageSource image;

        public ICommand TakeImageAsync => this.takeImageCommand;

        public ICommand SelectImageAsync => this.selectImageCommand;

        /// <inheritdoc />
        public ImageSource Image
        {
            get => this.image;
            set => SetProperty(ref this.image, value);
        }

        protected ImagePickerViewModel()
        {
            this.takeImageCommand = new ImageCommand(true, this);
            this.selectImageCommand = new ImageCommand(false, this);
        }

        private class ImageCommand : ICommand
        {
            private readonly bool takeImage;
            [NotNull]
            private readonly ImagePickerViewModel parentViewModel;

            internal ImageCommand(bool takeImage, [NotNull]ImagePickerViewModel parentViewModel)
            {
                this.takeImage = takeImage;
                this.parentViewModel = parentViewModel;
                this.parentViewModel.PropertyChanged += (sender, args) =>
                                                        {
                                                            if (args?.PropertyName == nameof(BaseViewModel.IsBusy))
                                                            {
                                                                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                                                            }
                                                        };
            }

            public bool CanExecute(object parameter)
            {
                return !this.parentViewModel.IsBusy;
            }

            public async void Execute(object parameter)
            {
                if (CanExecute(parameter))
                {
                    this.parentViewModel.IsBusy = true;
                    try
                    {
                        var imageService = Factory.Get<IImageService>();
                        var image = this.takeImage
                            ? await imageService.TakeImageAsync()
                            : await imageService.SelectImageAsync();
                        this.parentViewModel.Image = image;
                    }
                    catch(Exception error)
                    {
                        this.parentViewModel.RaiseOnError(error);
                    }

                    this.parentViewModel.IsBusy = false;
                }
            }

            public event EventHandler CanExecuteChanged;
        }
    }
}