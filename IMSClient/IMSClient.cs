using System;
using IMSClient.Helper;
using IMSClient.Page;
using IMSClient.Respository;
using Xamarin.Forms;

namespace IMSClient
{
    public class App : Application
    {
        private readonly IServerFinder _serverFinder;
        private NavigationPage _navigationPage;
        private DashboardPage _dashboardPage;
        private readonly IUserRepository _userRepository;

        public App(IUserRepository userRepository = null, IServerFinder serverFinder = null)
        {
            _serverFinder = serverFinder;
            _userRepository = userRepository ?? DependencyService.Get<IUserRepository>();

            MainPage = _navigationPage = new NavigationPage();

            if (_userRepository.IsLogged())
            {
                _dashboardPage = new DashboardPage(_userRepository);

                _navigationPage.PushAsync(_dashboardPage);
            }
            else
            {
                var loginPage = new LoginPage(_userRepository);

                loginPage.Login += Login;
                loginPage.Register += Register;

                _navigationPage.PushAsync(loginPage);
            }

        }

        private async void Register(object sender, RegisterEventArgs e)
        {
            var registerPage = new RegisterPage(_userRepository);
            
            registerPage.Registered += Registered;
            
            await _navigationPage.PushAsync(registerPage);
        }

        private async void Registered(object sender, RegisterEventArgs e)
        {
            _userRepository.GetUserLoginModel().Email = e.Email;

            await _navigationPage.PopAsync();
        }


        private void Login(object sender, LoginEventArgs loginEventArgs)
        {
            var dashboardPage = new DashboardPage(_userRepository);

            MainPage = _navigationPage = new NavigationPage(dashboardPage);
        }



        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

