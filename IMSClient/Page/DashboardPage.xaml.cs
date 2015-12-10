
using IMSClient.Respository;
using Xamarin.Forms;

namespace IMSClient.Page
{
    public partial class DashboardPage : ContentPage
    {
        private IUserRepository _userRepository;
        private IValuesRepository _valuesRepository;
        
        public DashboardPage(IUserRepository _userRepository = null, IValuesRepository _valuesRepository = null)
        {
            this._userRepository = _userRepository ?? DependencyService.Get<IUserRepository>(); ;
            this._valuesRepository = _valuesRepository?? DependencyService.Get<IValuesRepository>(); ;
            InitializeComponent();
        }
    }
}
