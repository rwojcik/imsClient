using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMSClient.Respository;
using Xamarin.Forms;

namespace IMSClient.Page
{
    public delegate void Registered(object sender, RegisterEventArgs e);

    public partial class RegisterPage : ContentPage
    {
        private readonly IUserRepository _userRepository;

        public Registered Registered;

        public RegisterPage(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? DependencyService.Get<IUserRepository>();

            InitializeComponent();
        }
    }
}
