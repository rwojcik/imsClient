using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IMSClient.Helper;
using IMSClient.IMSException;
using IMSClient.Model;
using IMSClient.Model.User;
using IMSClient.Page;
using IMSClient.Respository.Impl;
using IMSPrototyper.ViewModels;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserRespository))]
namespace IMSClient.Respository.Impl
{
    public class UserRespository : IUserRepository
    {
        private UserLoginModel _userLoginModel;
        private readonly IServerFinder _serverFinder;

        public UserRespository(IServerFinder serverFinder = null)
        {
            _serverFinder = serverFinder ?? DependencyService.Get<IServerFinder>();
        }

        public UserLoginModel GetUserLoginModel()
        {
            return _userLoginModel ?? (_userLoginModel = CreateLoginModel());
        }

        public async Task<UserLoginModel> LoginAsync(UserLoginModel userLoginModel)
        {
            var client = new RestClient($"http://{_serverFinder.GetServerAddress()}/");

            var request = new RestRequest("token", Method.POST);
            request.AddParameter("username", "prototype@test.com");
            request.AddParameter("password", "P@ssw0rd");
            request.AddParameter("grant_type", "password");

            var response = await client.Execute<Token>(request);

            if (!response.IsSuccess)
            {
                throw new RestRequestException("Request failed", new Exception(response.StatusDescription));
            }

            userLoginModel.Token = response.Data.access_token;
            userLoginModel.TokenType = response.Data.token_type;
            userLoginModel.TokenExpires = response.Data.expires;

            return userLoginModel;
        }

        public async Task RegisterAsync(UserRegisterModel userRegisterModel)
        {
            var client = new RestClient($"http://{_serverFinder.GetServerAddress()}/");

            var request = new RestRequest("api/Account/Register", Method.POST);
            request.AddJsonBody(new { Email = userRegisterModel.Email, Password = userRegisterModel.Password, ConfirmPassword = userRegisterModel.Password });
            
            var response = await client.Execute(request);

            if (!response.IsSuccess)
            {
                throw new RestRequestException("Request failed");
            }
        }

        private static UserLoginModel CreateLoginModel()
        {
            return new UserLoginModel();
        }

        public string GetTokenType()
        {
            return _userLoginModel.TokenType;
        }

        public string GetToken()
        {
            return _userLoginModel.Token;
        }

        public bool IsLogged()
        {
            return _userLoginModel?.TokenExpires != null && _userLoginModel.TokenExpires > DateTime.Now;
        }
    }
}
