using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMSClient.Extension;
using IMSClient.Extension.Impl;
using IMSClient.Model.User;
using IMSClient.Repository;
using Xamarin.Forms;

namespace IMSClient.Page
{
    public delegate void Registered(object sender, RegisterEventArgs e);

    public partial class RegisterPage : ContentPage
    {
        private readonly IUserRepository _userRepository;

        private readonly UserRegisterModel _userRegisterModel;

        private readonly INotifyPage _notifyPage;

        public event Registered Registered;

        public RegisterPage(IUserRepository userRepository = null)
        {
            _userRepository = userRepository ?? DependencyService.Get<IUserRepository>();

            _notifyPage = new NotifyPage(this);

            _userRegisterModel = new UserRegisterModel
            {
                Email = _userRepository?.GetUserLoginModel()?.Email ?? string.Empty,
            };

            BindingContext = _userRegisterModel;

            InitializeComponent();
        }

        private async void ButtonRegister(object sender, EventArgs e)
        {

            if (!AllFieldsFill())
            {
                ShowMissingFields();
                return;
            }
            if (!PasswordsEqual)
            {
                ShowPasswordsDifferent();
                return;
            }
            
            RegisterButton.IsEnabled = false;

            var logged = await _userRepository.RegisterAsync(_userRegisterModel);

            RegisterButton.IsEnabled = true;

            if (logged) Registered?.Invoke(this, new RegisterEventArgs());
            else _notifyPage.DisplayAlert("Registration error", "Please try again");
        }

        private void ShowPasswordsDifferent()
        {
            _notifyPage.DisplayAlert("Passwords mismatch", "Password and password confirmation fields should be equal.");
        }

        private void ShowMissingFields()
        {
            var missingFields = new List<string>(3);

            if (string.IsNullOrEmpty(_userRegisterModel.Email))
                missingFields.Add("email");

            if (string.IsNullOrEmpty(_userRegisterModel.Password))
                missingFields.Add("password");

            if (string.IsNullOrEmpty(_userRegisterModel.ConfirmPassword))
                missingFields.Add("password confirmation");

            string joinedFields = string.Join(", ", missingFields.Take(missingFields.Count - 1)) + (missingFields.Count <= 1 ? "" : " and ") + missingFields.LastOrDefault();

            var messageBuilder = new StringBuilder("Field");

            if (missingFields.Count > 1)
                messageBuilder.Append("s");

            messageBuilder.Append($" {joinedFields} ");

            messageBuilder.Append(missingFields.Count > 1 ? "are" : "is");

            messageBuilder.Append(" missing.");
            
            _notifyPage.DisplayAlert(missingFields.Count > 1 ? "Missing fields" : "Mising field", messageBuilder.ToString());
        }

        private bool AllFieldsFill()
            =>
                !string.IsNullOrEmpty(_userRegisterModel.Email) &&
                !string.IsNullOrEmpty(_userRegisterModel.ConfirmPassword) &&
                !string.IsNullOrEmpty(_userRegisterModel.Password);

        private bool PasswordsEqual =>
                _userRegisterModel.ConfirmPassword == _userRegisterModel.Password;
    }
}
