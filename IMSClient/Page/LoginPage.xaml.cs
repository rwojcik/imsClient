using System;
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

        public LoginPage(IUserRepository userRepository = null, IValuesRepository valuesRepository = null, INotifyPage notifyPage = null)
        {
            _userRepository = userRepository ?? DependencyService.Get<IUserRepository>();
            _valuesRepository = valuesRepository ?? DependencyService.Get<IValuesRepository>();
            _notifyPage = notifyPage ?? new NotifyPage(this);

            _userLoginModel = _userRepository.GetUserLoginModel();

            BindingContext = _userLoginModel;
            
            InitializeComponent();

            //Task.Factory.StartNew(() => ShowCredentialsButton.Text = _valuesRepository.GetValues().DefaultIfEmpty("errorrrr").FirstOrDefault());
        }

        private void ButtonShowCredentials(object sender, EventArgs e)
        {
            var msg = $"Username: {_userLoginModel.UserName}\nPassword: {_userLoginModel.Password}\nPassword saved: {_userLoginModel.SavePassord}";

            _notifyPage.DisplayAlert("Credentials", msg, "Dismiss");
        }

        private void ButtonLogin(object sender, EventArgs e)
        {
            if (Login == null)
            {
                _notifyPage.MissingHandler();
                return;
            }
            _userRepository.Login(_userLoginModel);
            
            Login(this, new LoginEventArgs(_userLoginModel));
            
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
        public UserLoginModel UserLoginModel { get; private set; }

        public LoginEventArgs(UserLoginModel userLoginModel)
        {
            UserLoginModel = userLoginModel;
        }
    }

    public class RegisterEventArgs : EventArgs
    {
        string UserName { get; set; }
    }
}
