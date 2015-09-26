using IMSClient.Model;
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

        private UserLoginModel CreateLoginModel()
        {
            return new UserLoginModel
            {
                UserName = "DI!"
            };
        }
    }
}
