using System.Threading.Tasks;

namespace IMSClient.Signal
{
    public interface IRealTimeService
    {
        event DeviceUpdated DeviceUpdated;
        Task<bool> UpdateDeviceAsync(DeviceSetting deviceSetting);
    }
}