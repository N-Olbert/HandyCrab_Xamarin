using System.ComponentModel;

namespace HandyCrab.Common.Interfaces
{
    public interface IViewModel : INotifyPropertyChanged
    {
        string PageTitle { get; }

        bool IsBusy { get; }
    }
}