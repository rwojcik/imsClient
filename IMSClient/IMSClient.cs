using System;
using IMSClient.Page;
using IMSClient.Respository;
using Xamarin.Forms;

namespace IMSClient
{
    public class App : Application
    {
        private readonly NavigationPage _navigationPage;
        private readonly IUserRepository _userRepository = DependencyService.Get<IUserRepository>();
        private readonly IValuesRepository _valuesRepository = DependencyService.Get<IValuesRepository>();

        public App()
        {
            var loginPage = new LoginPage(_userRepository, _valuesRepository);
            loginPage.Register += Register;
            loginPage.Login += Login;

            MainPage = _navigationPage = new NavigationPage(loginPage);
        }

        private void Login(object sender, LoginEventArgs loginEventArgs)
        {
            var dashboardPage = new DashboardPage();

            _navigationPage.PushAsync(dashboardPage);
        }

        private void Logged(object sender, EventArgs e)
        {

        }

        private void Register(object sender, RegisterEventArgs registerEventArgs)
        {
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

