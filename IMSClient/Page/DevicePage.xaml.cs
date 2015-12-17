using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMSClient.Repository;
using IMSClient.Signal;
using IMSClient.ViewModels;
using Xamarin.Forms;

namespace IMSClient.Page
{
    public partial class DevicePage : ContentPage
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly DeviceViewModel _deviceViewModel;
        private readonly IRealTimeService _realTimeService;

        public DevicePage(DeviceViewModel device, IDeviceRepository deviceRepository = null, IRealTimeService realTimeService = null)
        {
            _deviceViewModel = device;
            _deviceRepository = deviceRepository ?? DependencyService.Get<IDeviceRepository>();
            _realTimeService = realTimeService ?? DependencyService.Get<IRealTimeService>();

            BindingContext = _deviceViewModel;

            InitializeComponent();
        }

        private async void ChangeSettingButton_OnClicked(object sender, EventArgs e)
        {
            if (_deviceViewModel.Discriminator == "Binary")
            {

            }
            else if (_deviceViewModel.Discriminator == "Continous")
            {

            }
        }

        private async void UpdateModel()
        {
            var model = await _deviceRepository.GetDevice(_deviceViewModel.DeviceId);

            _deviceViewModel.BinarySetting = model.BinarySetting;
            _deviceViewModel.ContinousSetting = model.ContinousSetting;
            _deviceViewModel.Description = model.Description;
            _deviceViewModel.History = model.History;
        }
    }
}
