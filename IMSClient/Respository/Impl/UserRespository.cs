using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IMSClient.Helper;
using IMSClient.IMSException;
using IMSClient.Model.User;
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

        public async Task<UserLoginModel> LoginAsync()
        {
            string url = $"http://{_serverFinder.GetServerAddress()}/token";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://{_serverFinder.GetServerAddress()}/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("username", _userLoginModel.Email),
                    new KeyValuePair<string, string>("password", _userLoginModel.Password),
                    new KeyValuePair<string, string>("grant_type", "password")
                };

                var content = new FormUrlEncodedContent(postData);

                try
                {
                    var response = await client.PostAsync("token", content);

                    if(!response.IsSuccessStatusCode) return _userLoginModel;
                    
                    var result = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<Token>(result);

                    _userLoginModel.TokenType = token.token_type;
                    _userLoginModel.Token = token.access_token;
                    _userLoginModel.TokenExpires = token.expires;

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"type = {e.GetType()}, msg = {e.Message}");
                }

            }

            return _userLoginModel;
        }

        public async Task RegisterAsync()
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
