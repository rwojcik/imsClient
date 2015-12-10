using System.Threading.Tasks;
using IMSClient.Model;
using IMSClient.Model.User;

namespace IMSClient.Respository
{
    public interface IUserRepository
    {
        UserLoginModel GetUserLoginModel();
        Task<UserLoginModel> LoginAsync(UserLoginModel userLoginModel);

        Task RegisterAsync(UserRegisterModel userRegisterModel);
        string GetTokenType();
        string GetToken();

        bool IsLogged();
    }

    
}
