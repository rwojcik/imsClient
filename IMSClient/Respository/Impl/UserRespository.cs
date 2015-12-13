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

        public async Task<bool> LoginAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://{await _serverFinder.GetServerAddressAsync()}/");
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

                    if (!response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return false;
                    }
                    
                    var result = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<Token>(result);

                    _userLoginModel.TokenType = token.token_type;
                    _userLoginModel.Token = token.access_token;
                    _userLoginModel.TokenExpires = token.expires;

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"type = {e.GetType()}, msg = {e.Message}");
                    return false;
                }

            }

            return true;
        }

        public async Task<bool> RegisterAsync(UserRegisterModel userRegisterModel)
        {
            string url = $"http://{await _serverFinder.GetServerAddressAsync()}/api/Account/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(userRegisterModel);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync("Register", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"type = {e.GetType()}, msg = {e.Message}");
                    return false;
                }
            }

            return true;
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
