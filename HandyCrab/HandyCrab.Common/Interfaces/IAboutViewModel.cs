using System.Windows.Input;

namespace HandyCrab.Common.Interfaces
{
    public interface IAboutViewModel : IViewModel
    {
        string PageTitle { get; }

        string AboutText { get; }

        string GithubLink { get; }

        ICommand OpenGitHubLinkCommand { get; }
    }
}