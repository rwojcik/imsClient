using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IMSClient.HttpClient;
using IMSClient.Respository.Impl;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(ValuesRepository))]
namespace IMSClient.Respository.Impl
{
    class ValuesRepository : IValuesRepository
    {
        public IEnumerable<string> GetValues()
        {
            return new[] { "value1", "value2" }.AsEnumerable();
        }

        public async Task<IEnumerable<string>> GetRestValuesAsync()
        {
            var httpClient = DependencyService.Get<IGetHttpClient>().GetHttpClient();

            string url = "http://rwojcik-ims.azurewebsites.net/api/Values";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new string[0];

            var jsonString = await response.Content.ReadAsStringAsync();

            var vals = JsonConvert.DeserializeObject<List<string>>(jsonString);

            return vals;
        }
    }
}