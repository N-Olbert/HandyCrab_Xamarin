using System.Windows.Input;

namespace HandyCrab.Common.Interfaces
{
    public interface IAboutViewModel : IViewModel
    {

        ICommand OpenGitHubLinkCommand { get; }
    }
}