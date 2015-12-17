using System;
using IMSClient.Helper;
using IMSClient.Page;
using IMSClient.Repository;
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


        private async void Login(object sender, LoginEventArgs e)
        {
            var dashboardPage = new DashboardPage(_userRepository);
            dashboardPage.GroupChoose += DashboardPageOnGroupChoose;

            MainPage = _navigationPage = new NavigationPage(dashboardPage);
        }

        private async void DashboardPageOnGroupChoose(object sender, GroupChooseEventArgs e)
        {
            var groupPage = new GroupPage(e.Group);

            groupPage.DeviceChoose += GroupPage_DeviceChoose;
            
            await _navigationPage.PushAsync(groupPage);
        }

        private async void GroupPage_DeviceChoose(object sender, DeviceChooseEventArgs e)
        {
            var devicePage = new DevicePage(e.DeviceViewModel);

            await _navigationPage.PushAsync(devicePage);
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

