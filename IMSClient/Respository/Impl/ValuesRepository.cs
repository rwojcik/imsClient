using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using IMSClient.Helper;
using IMSClient.Respository.Impl;
using Xamarin.Forms;
using Newtonsoft.Json;

[assembly: Dependency(typeof(ValuesRepository))]
namespace IMSClient.Respository.Impl
{
    public class ValuesRepository : IValuesRepository
    {
        private readonly IServerFinder _serverFinder;
        private readonly IUserRepository _userRepository;

        public ValuesRepository() : this(null, null) { }

        public ValuesRepository(IServerFinder serverFinder = null, IUserRepository userRepository = null)
        {
            _serverFinder = serverFinder ?? DependencyService.Get<IServerFinder>();
            _userRepository = userRepository ?? DependencyService.Get<IUserRepository>();
        }

        public IEnumerable<string> GetValues()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetRestValuesAsync()
        {
            //var client = new RestClient($"http://{_serverFinder.GetServerAddress()}/");

            //var request = new RestRequest("api/Value", Method.GET);
            //request.AddParameter("Authorization", $"{_userRepository.GetTokenType()} {_userRepository.GetToken()}");

            //var response = client.Execute(request);

            //if (!response.IsSuccess)
            //{
            //    throw new RestRequestException("Request failed");
            //}

            //return null;

            string url = $"http://{await _serverFinder.GetServerAddressAsync()}/api/Values";

            var request = WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "GET";
            var serializer = new JsonSerializer();

            using (var response = await request.GetResponseAsync())
            using (var stream = response.GetResponseStream())
            using (var sr = new StreamReader(stream))
            using (var jsonTxtReader = new JsonTextReader(sr))
            {
                return await Task.Factory.StartNew(() => serializer.Deserialize<List<string>>(jsonTxtReader));
            }
        }
    }
}