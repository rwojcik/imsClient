using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IMSClient.Droid.Dependency;
using IMSClient.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(ServerFinder))]
namespace IMSClient.Droid.Dependency
{
    public class ServerFinder : IServerFinder
    {
        private string _serverAddress = String.Empty;

        private Task<string> _serverFinderTask;

        public ServerFinder()
        {
            _serverFinderTask = Task<string>.Factory.StartNew(DiscoverServerAddress);
        }

        public async Task<string> GetServerAddressAsync()
        {
            if (!_serverFinderTask.IsCompleted)
                await _serverFinderTask;

            return _serverAddress;
        }

        public bool ServerFound()
        {
            return _serverFinderTask.IsCompleted && !string.IsNullOrEmpty(_serverAddress);
        }

        private string DiscoverServerAddress()
        {
            try
            {
                var client = new UdpClient();
                var requestData = Encoding.ASCII.GetBytes("SomeRequestData");
                var serverEp = new IPEndPoint(IPAddress.Any, 0);

                client.EnableBroadcast = true;
                client.Send(requestData, requestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

                var serverResponseDataTask = Task<byte[]>.Factory.StartNew(() => client.Receive(ref serverEp));

                var timeout = DateTime.Now.AddSeconds(10d);

                while (timeout > DateTime.Now && !serverResponseDataTask.IsCompleted) ;

                if (!serverResponseDataTask.IsCompleted)
                {
                    return _serverAddress = "192.168.56.60:80";
                }

                var serverResponse = Encoding.ASCII.GetString(serverResponseDataTask.Result);

                _serverAddress = $"{serverEp.Address.ToString()}:80";
            }
            catch (Exception e)
            {
                Debug.WriteLine($"ServerFinder.DiscoverServerAddress: exception type: {e.GetType()}, msg: {e.Message}");
                return _serverAddress = "192.168.56.60:80";
            }
            Debug.WriteLine($"ServerFinder.DiscoverServerAddress: found server at {_serverAddress}");
            return _serverAddress;
        }
    }
}