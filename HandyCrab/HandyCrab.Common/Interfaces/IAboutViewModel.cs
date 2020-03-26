using System.Windows.Input;

namespace HandyCrab.Common.Interfaces
{
    public interface IAboutViewModel : IViewModel
    {
        string AboutText { get; }

        string GithubLink { get; }

        ICommand OpenGitHubLinkCommand { get; }
    }
}