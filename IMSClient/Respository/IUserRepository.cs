using IMSClient.Model;
using IMSClient.Model.User;

namespace IMSClient.Respository
{
    public interface IUserRepository
    {
        UserLoginModel GetUserLoginModel();
        void Login(UserLoginModel userLoginModel);
    }

    
}
