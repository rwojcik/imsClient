using System.Threading.Tasks;
using IMSClient.Model;
using IMSClient.Model.User;

namespace IMSClient.Repository
{
    public interface IUserRepository
    {
        UserLoginModel GetUserLoginModel();

        Task<bool> LoginAsync();

        Task<bool> RegisterAsync(UserRegisterModel userRegisterModel);

        string GetTokenType();

        string GetToken();
        
        bool IsLogged();
    }

    
}
