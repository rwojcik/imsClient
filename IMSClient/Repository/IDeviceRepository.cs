using System.Collections.Generic;
using System.Threading.Tasks;
using IMSClient.ViewModels;

namespace IMSClient.Repository
{
    public interface IDeviceRepository
    {
        Task<IList<DeviceViewModel>> GetDevicesAsync(long groupId);

        Task<DeviceViewModel> GetDevice(long deviceId);

        Task<bool> PutDeviceAsync(UpdateDeviceViewModel updateDeviceViewModel);
    }
}