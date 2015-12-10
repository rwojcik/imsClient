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
using Xamarin.Forms;

[assembly: Dependency(typeof(UserRespository))]
namespace IMSClient.Respository.Impl
{
    public class UserRespository : IUserRepository
    {
        private UserLoginModel _userLoginModel;
        private readonly IServerFinder _serverFinder;

        public UserRespository() : this(null)
        {
        }

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
            //var serverAddress = $"http://{_serverFinder.GetServerAddress()}/";

            //var client = new RestClient(serverAddress);

            //var request = new RestRequest("token", Method.POST);
            //request.AddParameter("username", "prototype@test.com");
            //request.AddParameter("password", "P@ssw0rd");
            //request.AddParameter("grant_type", "password");

            //try
            //{

            //    var response = client.Execute<Token>(request);

            //    var data = response.Result;

            //    if (!data.IsSuccess)
            //    {
            //        throw new RestRequestException($"Request failed, {response.Result.StatusDescription}");
            //    }

            //    userLoginModel.Token = data.Data.access_token;
            //    userLoginModel.TokenType = data.Data.token_type;
            //    userLoginModel.TokenExpires = data.Data.expires;

            //    return userLoginModel;

            //}
            //catch (Exception e)
            //{
            //    throw;
            //}

            return null;
        }

        public async Task RegisterAsync(UserRegisterModel userRegisterModel)
        {
            //var client = new RestClient($"http://{_serverFinder.GetServerAddress()}/");

            //var request = new RestRequest("api/Account/Register", Method.POST);
            //request.AddJsonBody(new { Email = userRegisterModel.Email, Password = userRegisterModel.Password, ConfirmPassword = userRegisterModel.Password });
            
            //var response = await client.Execute(request);

            //if (!response.IsSuccess)
            //{
            //    throw new RestRequestException("Request failed");
            //}

            throw new Exception();
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
