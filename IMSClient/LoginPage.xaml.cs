using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace IMSClient
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginData _loginData;

        public LoginPage()
        {
            _loginData = new LoginData();
            BindingContext = _loginData;
            InitializeComponent();
        }

        private void ButtonShowCredentials(object sender, EventArgs e)
        {
            var msg = string.IsNullOrEmpty(_loginData?.Password) || string.IsNullOrEmpty(_loginData?.UserName)
                ? string.Empty
                : $"{_loginData.UserName}\n{_loginData.Password}";

            DisplayAlert("Credentials", msg, "Dismiss");
        }

        private void ButtonLogin(object sender, EventArgs e)
        {
            DisplayAlert("Error", "Not yet implemented...", "Dismiss");
        }
    }
}
