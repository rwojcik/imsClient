﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IMSClient.Extension;
using IMSClient.Extension.Impl;
using IMSClient.Model;
using IMSClient.Model.User;
using IMSClient.Respository;
using Xamarin.Forms;

namespace IMSClient.Page
{
    public delegate void Login(object sender, LoginEventArgs e);
    public delegate void Register(object sender, RegisterEventArgs e);

    public partial class LoginPage : ContentPage
    {
        private readonly UserLoginModel _userLoginModel;
        private readonly IUserRepository _userRepository;
        private readonly INotifyPage _notifyPage;
        private readonly IValuesRepository _valuesRepository;
        public Register Register;
        public Login Login;

        public LoginPage(IUserRepository userRepository = null, INotifyPage notifyPage = null, IValuesRepository valuesRepository = null)
        {
            _userRepository = userRepository ?? DependencyService.Get<IUserRepository>();
            _notifyPage = notifyPage ?? new NotifyPage(this);
            _valuesRepository = valuesRepository ?? DependencyService.Get<IValuesRepository>();

            _userLoginModel = _userRepository.GetUserLoginModel();

            BindingContext = _userLoginModel;
            
            InitializeComponent();
        }

        private void ButtonShowCredentials(object sender, EventArgs e)
        {
            var msg = $"Username: {_userLoginModel.Email}\nPassword: {_userLoginModel.Password}\nPassword saved: {_userLoginModel.SavePassord}";

            _notifyPage.DisplayAlert("Credentials", msg);
        }

        private async void ButtonLogin(object sender, EventArgs e)
        {
            LoginButton.IsEnabled = false;
            RegisterButton.IsEnabled = false;

            try
            {
                var test = await _valuesRepository.GetRestValuesAsync();
                Debug.WriteLine(string.Join(", ", test));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            
            if (Login == null)
            {
                _notifyPage.MissingHandler();
                return;
            }

            var logged = await _userRepository.LoginAsync();

            LoginButton.IsEnabled = true;
            RegisterButton.IsEnabled = true;

            if (logged)
            {

                Login(this, new LoginEventArgs(_userLoginModel));
            }
            else
            {
                _notifyPage.DisplayAlert("Error", "Incorrect credentials");
            }

        }

        private void ButtonRegister(object sender, EventArgs e)
        {
            if (Register == null)
            {
                _notifyPage.MissingHandler();
                return;
            }
            
            Register(this, new RegisterEventArgs());
        }
    }

    public class LoginEventArgs : EventArgs
    {
        public UserLoginModel UserLoginModel { get; }

        public LoginEventArgs(UserLoginModel userLoginModel)
        {
            UserLoginModel = userLoginModel;
        }
    }

    public class RegisterEventArgs : EventArgs
    {
        public string Email { get; set; }
    }
}
