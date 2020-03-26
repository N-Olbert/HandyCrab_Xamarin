using System.Windows.Input;
using HandyCrab.Business.Ressources;
using HandyCrab.Common.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandyCrab.Business.ViewModels
{
    internal class AboutViewModel : BaseViewModel, IAboutViewModel
    {
        public string AboutText => Strings.AboutPageContentText;

        public string GithubLink => Strings.GitHubProjectLink;

        public ICommand OpenGitHubLinkCommand { get; }

        public override string PageTitle => Strings.AboutPageTitle;

        public AboutViewModel()
        {
            OpenGitHubLinkCommand = new Command(async () => await Browser.OpenAsync(Strings.GitHubProjectLink));
        }
    }
}