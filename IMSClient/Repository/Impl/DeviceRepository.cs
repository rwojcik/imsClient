using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IMSClient.Helper;
using IMSClient.Repository.Impl;
using IMSClient.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceRepository))]
namespace IMSClient.Repository.Impl
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IServerFinder _serverFinder;

        public DeviceRepository() : this(null, null) { }

        public DeviceRepository(IServerFinder serverFinder = null, IUserRepository userRepository = null)
        {
            _serverFinder = serverFinder ?? DependencyService.Get<IServerFinder>();
            _userRepository = userRepository ?? DependencyService.Get<IUserRepository>();
        }

        public async Task<DeviceViewModel> GetDevice(long deviceId)
        {
            if (!_userRepository.IsLogged() || deviceId < 1) return null;

            string url = $"http://{await _serverFinder.GetServerAddressAsync()}/api/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_userRepository.GetTokenType(), _userRepository.GetToken());

                try
                {
                    var response = await client.GetAsync($"Device/{deviceId}");
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"Unsuccessful GET for /Device/{deviceId}/, {response.StatusCode}");
                        return null;
                    }

                    var device = JsonConvert.DeserializeObject<DeviceViewModel>(responseString);
                    return device;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"type = {e.GetType()}, msg = {e.Message}");
                    return null;
                }
            }
        }

        public async Task<bool> PutDeviceAsync(UpdateDeviceViewModel updateDeviceViewModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://{await _serverFinder.GetServerAddressAsync()}/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_userRepository.GetTokenType(), _userRepository.GetToken());

                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(nameof(updateDeviceViewModel.DeviceId), updateDeviceViewModel.DeviceId.ToString()),
                    new KeyValuePair<string, string>(nameof(updateDeviceViewModel.Name), updateDeviceViewModel.Name),
                    new KeyValuePair<string, string>(nameof(updateDeviceViewModel.Description), updateDeviceViewModel.Description),
                    new KeyValuePair<string, string>(nameof(updateDeviceViewModel.GroupId), updateDeviceViewModel.GroupId.ToString()),
                    new KeyValuePair<string, string>(nameof(updateDeviceViewModel.Discriminator), updateDeviceViewModel.Discriminator),
                    new KeyValuePair<string, string>(nameof(updateDeviceViewModel.ContinousSetting), updateDeviceViewModel.ContinousSetting.ToString()),
                    new KeyValuePair<string, string>(nameof(updateDeviceViewModel.BinarySetting), updateDeviceViewModel.BinarySetting.ToString()),
                };

                var content = new FormUrlEncodedContent(postData);

                try
                {
                    var response = await client.PutAsync($"Device/{updateDeviceViewModel.DeviceId}", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Unsuccesful PUT for device {updateDeviceViewModel.DeviceId}, repsonse: {responseContent}");
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

        public async Task<IList<DeviceViewModel>> GetDevicesAsync(long groupId)
        {
            if (!_userRepository.IsLogged() || groupId == 0) return new List<DeviceViewModel>(0);

            string url = $"http://{await _serverFinder.GetServerAddressAsync()}/api/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_userRepository.GetTokenType(), _userRepository.GetToken());

                try
                {
                    var response = await client.GetAsync($"Device?groupId={groupId}");

                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"Unsuccessful GET for /Device/, {response.StatusCode}");
                        return new List<DeviceViewModel>(0);
                    }

                    var devices = JsonConvert.DeserializeObject<IList<DeviceViewModel>>(responseString);
                    return devices;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"type = {e.GetType()}, msg = {e.Message}");
                    return new List<DeviceViewModel>(0);
                }
            }
        }
    }
}