using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using Taut.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Taut
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private ApplicationViewModel AppViewModel
        {
            get
            {
                return (Application.Current as App).ViewModel;
            }
        }

        public WelcomePage()
        {
            this.InitializeComponent();
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(640, 480));
            ApplicationView.PreferredLaunchViewSize = new Size(640, 480);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private async void HandleSignInClick(object sender, RoutedEventArgs e)
        {
            string clientId = "";
            string scope = String.Join(' ', new string[] {
                "channels:read",
                "team:read",
                "users.profile:read",
                "users:read" });
            string redirectUri = "https://bing.com";
            var startUri = new Uri($"https://slack.com/oauth/authorize" +
                $"?client_id={HttpUtility.UrlEncode(clientId)}" + 
                $"&scope={HttpUtility.UrlEncode(scope)}" + 
                $"&redirect_uri={HttpUtility.UrlEncode(redirectUri)}");
            var endUri = new Uri(redirectUri);
            var webAuthResult = 
                await WebAuthenticationBroker.AuthenticateAsync(
                    WebAuthenticationOptions.None, startUri, endUri);
            switch (webAuthResult.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    var successMessage = webAuthResult.ResponseData.ToString();
                    Frame.Navigate(typeof(MainPage));
                    break;
                case WebAuthenticationStatus.ErrorHttp:
                    ErrorMessage = webAuthResult.ResponseErrorDetail.ToString();
                    break;
                default:
                    ErrorMessage = webAuthResult.ResponseData.ToString();
                    break;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
