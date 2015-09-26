using System;
using System.Linq;
using System.Threading.Tasks;
using IMSClient.Extension;
using IMSClient.Model;
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
        public Register Register;
        public Login Login;
        
        public LoginPage(IUserRepository userRepository, IValuesRepository valuesRepository)
        {
            _userRepository = userRepository;
            _userLoginModel = userRepository.GetUserLoginModel();
            
            BindingContext = _userLoginModel;
            InitializeComponent();

            //Task.Factory.StartNew(() => ShowCredentialsButton.Text = valuesRepository.GetRestValuesAsync().Result.FirstOrDefault());
        }

        private void ButtonShowCredentials(object sender, EventArgs e)
        {
            var msg = $"Username: {_userLoginModel.UserName}\nPassword: {_userLoginModel.Password}\nPassword saved: {_userLoginModel.SavePassord}";

            DisplayAlert("Credentials", msg, "Dismiss");
        }

        private void ButtonLogin(object sender, EventArgs e)
        {
            if (Login != null)
                Login(this, new LoginEventArgs(_userLoginModel));
            else
                this.MissingHandler();
        }

        private void ButtonRegister(object sender, EventArgs e)
        {
            if (Register != null)
                Register(this, new RegisterEventArgs());
            else
                this.MissingHandler();
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

    }
}
