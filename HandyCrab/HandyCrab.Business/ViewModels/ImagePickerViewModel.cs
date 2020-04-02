using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HandyCrab.Business.Services;
using HandyCrab.Common.Interfaces;
using JetBrains.Annotations;
using Xamarin.Forms;

namespace HandyCrab.Business.ViewModels
{
    internal class ImagePickerViewModel : BaseViewModel, IImagePickerViewModel
    {
        private readonly ImageCommand takeImageCommand;
        private readonly ImageCommand selectImageCommand;
        public ICommand TakeImageAsync => this.takeImageCommand;

        public ICommand SelectImageAsync => this.selectImageCommand;

        public ImagePickerViewModel()
        {
            this.takeImageCommand = new ImageCommand(true, this);
            this.selectImageCommand = new ImageCommand(false, this);
        }

        private class ImageCommand : ICommand
        {
            private readonly bool takeImage;
            [NotNull]
            private readonly BaseViewModel parentViewModel;

            internal ImageCommand(bool takeImage, [NotNull]BaseViewModel parentViewModel)
            {
                this.takeImage = takeImage;
                this.parentViewModel = parentViewModel;
            }

            public bool CanExecute(object parameter)
            {
                return !this.parentViewModel.IsBusy && parameter is Image;
            }

            public async void Execute(object parameter)
            {
                if (CanExecute(parameter))
                {
                    this.parentViewModel.IsBusy = true;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                    try
                    {
                        var image = this.takeImage
                            ? await ImageService.TakeImageAsync()
                            : await ImageService.SelectImageAsync();
                        ((Image)parameter).Source = image;
                    }
                    catch
                    {
                    }

                    this.parentViewModel.IsBusy = false;
                }

                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            public event EventHandler CanExecuteChanged;
        }
    }
}