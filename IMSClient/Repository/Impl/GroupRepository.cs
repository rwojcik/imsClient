using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IMSClient.Helper;
using IMSClient.Repository.Impl;
using IMSClient.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(GroupRepository))]
namespace IMSClient.Repository.Impl
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IServerFinder _serverFinder;

        public GroupRepository() : this(null, null)
        {
        }

        public GroupRepository(IUserRepository userRepository = null, IServerFinder serverFinder = null)
        {
            _serverFinder = serverFinder ?? DependencyService.Get<IServerFinder>();
            _userRepository = userRepository ?? DependencyService.Get<IUserRepository>();
        }

        public async Task<IList<GroupViewModel>> GetGroupsAsync()
        {
            if (!_userRepository.IsLogged()) return new List<GroupViewModel>(0);
            
            string url = $"http://{await _serverFinder.GetServerAddressAsync()}/api/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_userRepository.GetTokenType(), _userRepository.GetToken());
                
                try
                {
                    var response = await client.GetAsync("Group");

                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"Unsuccessful GET for /Group/, {response.StatusCode}");
                        return new List<GroupViewModel>(0);
                    }

                    var groups = JsonConvert.DeserializeObject<IList<GroupViewModel>>(responseString);
                    return groups;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"type = {e.GetType()}, msg = {e.Message}");
                    return new List<GroupViewModel>(0);
                }
            }
        }
    }
}