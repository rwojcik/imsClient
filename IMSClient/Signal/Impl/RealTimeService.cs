using System;
using System.Diagnostics;
using System.Threading.Tasks;
using IMSClient.Helper;
using IMSClient.Signal.Impl;
using Microsoft.AspNet.SignalR.Client;
using Xamarin.Forms;

[assembly: Dependency(typeof(RealTimeService))]
namespace IMSClient.Signal.Impl
{
    public class RealTimeService : IRealTimeService
    {
        private readonly IServerFinder _serverFinder;
        private HubConnection _hubConnection;
        private IHubProxy _hubProxy;

        public event DeviceUpdated DeviceUpdated;

        public RealTimeService() : this(null) { }

        public RealTimeService(IServerFinder serverFinder = null)
        {
            _serverFinder = serverFinder ?? DependencyService.Get<IServerFinder>();

            Task.Factory.StartNew(OpenConnection);
        }

        private async Task OpenConnection()
        {
            var url = $"http://{await _serverFinder.GetServerAddressAsync()}/";

            try
            {
                _hubConnection = new HubConnection(url);

                _hubProxy = _hubConnection.CreateHubProxy("device");
                _hubProxy.On<string>("hello", message => Hello(message));
                _hubProxy.On("helloMsg", () => Hello("EMPTY"));
                _hubProxy.On<long, bool, bool>("binaryDeviceUpdated", (deviceId, success, binarySetting) => InvokeDeviceUpdated(deviceId, success, binarySetting: binarySetting));
                _hubProxy.On<long, bool, double>("continousDeviceUpdated", (deviceId, success, continousSetting) => InvokeDeviceUpdated(deviceId, success, continousSetting));

                await _hubConnection.Start();

                await _hubProxy.Invoke("helloMsg", "mobile device here");

                Debug.WriteLine($"{nameof(RealTimeService)}.{nameof(OpenConnection)} SignalR connection opened");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(RealTimeService)}.{nameof(OpenConnection)} ex: {e.GetType()}, msg: {e.Message}");
            }
        }

        private void InvokeDeviceUpdated(long deviceId, bool success, double continousSetting = default(double), bool binarySetting = default(bool))
        {
            Debug.WriteLine($"{nameof(RealTimeService)}.{nameof(UpdateDeviceAsync)} new msg - id:{deviceId}, success:{success}, cSett:{continousSetting}, bSett:{binarySetting}");

            DeviceUpdated?.Invoke(this, new DeviceSetting(deviceId, string.Empty, success, continousSetting, binarySetting));
        }

        private void Hello(string message)
        {
            Debug.WriteLine($"New SignalR message: {message}");
        }

        public async Task<bool> UpdateDeviceAsync(DeviceSetting deviceSetting)
        {
            try
            {
                if (deviceSetting.Discriminator == "Binary")
                {
                    await _hubProxy.Invoke("binaryDeviceUpdate", deviceSetting.DeviceId, deviceSetting.BinarySetting);

                    Debug.WriteLine($"{nameof(RealTimeService)}.{nameof(UpdateDeviceAsync)} binary device updated, setting {deviceSetting.BinarySetting}");
                    return true;
                }

                if (deviceSetting.Discriminator == "Continous")
                {
                    await
                        _hubProxy.Invoke("continousDeviceUpdate", deviceSetting.DeviceId, deviceSetting.ContinousSetting);

                    Debug.WriteLine($"{nameof(RealTimeService)}.{nameof(UpdateDeviceAsync)} continous device updated, setting {deviceSetting.ContinousSetting}");
                    return true;
                }

                Debug.WriteLine($"{nameof(RealTimeService)}.{nameof(UpdateDeviceAsync)} unknown discriminator");
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(RealTimeService)}.{nameof(UpdateDeviceAsync)} ex: {e.GetType()}, msg: {e.Message}");
                return false;
            }
        }
    }
}