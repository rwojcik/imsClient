using IMSClient.Model;
using IMSClient.Model.User;
using IMSClient.Respository.Impl;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserRespository))]
namespace IMSClient.Respository.Impl
{
    public class UserRespository : IUserRepository
    {
        private UserLoginModel _userLoginModel;

        public UserLoginModel GetUserLoginModel()
        {
            return _userLoginModel ?? (_userLoginModel = CreateLoginModel());
        }

        public void Login(UserLoginModel userLoginModel)
        {
            var url = "http://rwojcik-ims.azurewebsites.net/token";
            


        }

        public void Register(UserRegisterModel userRegisterModel)
        {
            var url = "http://rwojcik-ims.azurewebsites.net/api/Account/Register";
        }

        private static UserLoginModel CreateLoginModel()
        {
            return new UserLoginModel();
        }
    }
}
