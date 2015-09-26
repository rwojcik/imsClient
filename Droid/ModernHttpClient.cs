using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ModernHttpClient;
using System.Net.Http;
using IMSClient.Droid;
using IMSClient.HttpClient;
using Xamarin.Forms;

[assembly: Dependency(typeof(IMSClient.Droid.ModernHttpClient))]
namespace IMSClient.Droid
{
    public class ModernHttpClient :IGetHttpClient
    {
        
        public ModernHttpClient()
        {
            var handler = new NativeMessageHandler();

            var httpClient = new System.Net.Http.HttpClient(handler);
        }

        public 
    }
}