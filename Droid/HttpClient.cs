using IMSClient.Droid;
using IMSClient.HttpClient;
using ModernHttpClient;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidHttpClient))]
namespace IMSClient.Droid
{
    public class DroidHttpClient : IGetHttpClient
    {
        public System.Net.Http.HttpClient HttpClient { get; private set; }

        public DroidHttpClient()
        {
            HttpClient = new System.Net.Http.HttpClient(new NativeMessageHandler());
        }


        public System.Net.Http.HttpClient GetHttpClient()
        {
            return HttpClient;
        }
    }
}