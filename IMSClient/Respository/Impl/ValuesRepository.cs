using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IMSClient.Helper;
using IMSClient.IMSException;
using IMSClient.Respository.Impl;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.Deserializers;
using RestSharp.Portable.HttpClient;
using Xamarin.Forms;

[assembly: Dependency(typeof(ValuesRepository))]
namespace IMSClient.Respository.Impl
{
    public class ValuesRepository : IValuesRepository
    {
        private readonly IServerFinder _serverFinder;
        private readonly IUserRepository _userRepository;

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
            var client = new RestClient($"http://{_serverFinder.GetServerAddress()}/");

            var request = new RestRequest("api/Value", Method.GET);
            request.AddParameter("Authorization", $"{_userRepository.GetTokenType()} {_userRepository.GetToken()}");

            var response = await client.Execute<List<string>>(request);

            if (!response.IsSuccess)
            {
                throw new RestRequestException("Request failed");
            }

            return response.Data;
        }
    }
}