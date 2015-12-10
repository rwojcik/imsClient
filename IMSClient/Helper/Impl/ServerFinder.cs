using IMSClient.Helper.Impl;
using Xamarin.Forms;

[assembly: Dependency(typeof(ServerFinder))]
namespace IMSClient.Helper.Impl
{
    
    public class ServerFinder :IServerFinder
    {
        public string GetServerAddress()
        {
            return "192.168.56.60:80";
        }
    }
}