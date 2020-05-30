using System.Net;
using System.Net.Http;
using HandyCrab.Common.Entitys;
using Xamarin.Forms;

namespace HandyCrab.UI.Views
{
    public abstract class BaseContentPage : ContentPage
    {
        protected void OnError(object sender, Failable e)
        {
            switch (e?.ErrorCode)
            {
                case 2:
                    DisplayAlert(Strings.Error, Strings.Error_NotLoggedIn, "OK");
                    break;
                case 3:
                    DisplayAlert(Strings.Error, Strings.Error_MailAlreadyInUse, "OK");
                    break;
                case 4:
                    DisplayAlert(Strings.Error, Strings.Error_UsernameAlreadyInUse, "OK");
                    break;
                case 5:
                    DisplayAlert(Strings.Error, Strings.Error_InvalidMail, "OK");
                    break;
                case 6:
                case 12:
                case 13:
                    DisplayAlert(Strings.Error, Strings.Error_WrongLogin, "OK");
                    break;
                case 9:
                    DisplayAlert(Strings.Error, Strings.Error_BarrierNotFound, "OK");
                    break;
                case 11:
                    DisplayAlert(Strings.Error, Strings.Error_SolutionNotFound, "OK");
                    break;
                case 14:
                case 15:
                    DisplayAlert(Strings.Error, Strings.Error_InvalidPictureFormat, "OK");
                    break;
                default:
                    if (e?.ThrownException is HttpRequestException || e?.ThrownException is HttpListenerException)
                    {
                        DisplayAlert(Strings.Error, Strings.Error_NetworkTimeout, "OK");
                        break;
                    }

                    DisplayAlert(Strings.Error, Strings.Error_UnknownError + e?.ThrownException?.Message, "OK");
                    break;
            }
        }
    }
}