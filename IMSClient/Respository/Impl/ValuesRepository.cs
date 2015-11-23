using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IMSClient.Respository.Impl;
using RestSharp.Portable;
using Xamarin.Forms;

[assembly: Dependency(typeof(ValuesRepository))]
namespace IMSClient.Respository.Impl
{
    public class ValuesRepository : IValuesRepository
    {
        public IEnumerable<string> GetValues()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetRestValuesAsync()
        {
            string url = "http://rwojcik-ims.azurewebsites.net/api/Values";

            using (var client = new RestClient(new Uri(url)))
            {
                var request = new RestRequest("ticker", HttpMethod.Get);
                var result = await client.Execute<List<string>>(request);

                return result.Data;
            }

        }
    }
}