using System.Windows.Input;
using HandyCrab.Common.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandyCrab.Business.ViewModels
{
    internal class AboutViewModel : BaseViewModel, IAboutViewModel
    {
        public ICommand OpenGitHubLinkCommand { get; }
        public AboutViewModel()
        {
            OpenGitHubLinkCommand = new Command<string>(async (string GithubProjectLink) => await Browser.OpenAsync(GithubProjectLink));
        }
    }
}