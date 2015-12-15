using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IMSClient.Helper;
using IMSClient.Model.User;
using IMSClient.Repository.Impl;
using IMSClient.ViewModels;
using IMSPrototyper.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserRepository))]
namespace IMSClient.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private UserLoginModel _userLoginModel;
        private readonly IServerFinder _serverFinder;

        public UserRepository() : this(null)
        {
        }

        public UserRepository(IServerFinder serverFinder = null)
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
                        Debug.WriteLine($"Error logging in, response : {responseContent}");
                        return false;
                    }

                    var result = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<Token>(result.Replace(".expires", nameof(Token.Expires)).Replace("token_type", nameof(Token.TokenType)).Replace("access_token", nameof(Token.AccessToken)));
                    
                    _userLoginModel.TokenType = token.TokenType;
                    _userLoginModel.Token = token.AccessToken;
                    _userLoginModel.TokenExpires = token.Expires;

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
            return new UserLoginModel
            {
                Email = "rafalwojcik3@gmail.com",
                Password = "P@ssw0rd"
            };
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
            var logged = _userLoginModel?.TokenExpires != null && _userLoginModel.TokenExpires > DateTime.Now && !string.IsNullOrWhiteSpace(_userLoginModel?.Token);
            return logged;
        }
    }
}
