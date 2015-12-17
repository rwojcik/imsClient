using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMSClient.Extension;
using IMSClient.Extension.Impl;
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
        private readonly INotifyPage _notifyPage;

        public DevicePage(DeviceViewModel device, IDeviceRepository deviceRepository = null, IRealTimeService realTimeService = null)
        {
            _deviceViewModel = device;
            _deviceRepository = deviceRepository ?? DependencyService.Get<IDeviceRepository>();
            _realTimeService = realTimeService ?? DependencyService.Get<IRealTimeService>();
            _notifyPage = new NotifyPage(this);
            _realTimeService.DeviceUpdated += RealTimeService_DeviceUpdated;

            BindingContext = _deviceViewModel;

            InitializeComponent();

            if (device.DeviceType == DeviceType.AutomaticWindow || device.DeviceType == DeviceType.Thermostat)
                ChangeSettingButton.IsVisible = true;

            if (device.DeviceType == DeviceType.Thermostat)
                ContinousSettingEntry.IsVisible = true;

        }

        private void RealTimeService_DeviceUpdated(object sender, DeviceSetting deviceSetting)
        {
            Debug.WriteLine($"{nameof(DevicePage)}.{nameof(RealTimeService_DeviceUpdated)}, id: {deviceSetting.DeviceId}, handling: {deviceSetting.DeviceId == _deviceViewModel.DeviceId}");

            Device.BeginInvokeOnMainThread(() => DeviceUpdated_MainThread(deviceSetting));
        }

        private async Task DeviceUpdated_MainThread(DeviceSetting deviceSetting)
        {
            if (deviceSetting.DeviceId == _deviceViewModel.DeviceId)
            {
                try
                {
                    if (ChangeSettingButton.IsVisible && !ChangeSettingButton.IsEnabled)
                    {
                        ChangeSettingButton.IsEnabled = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(
                        $"{nameof(DevicePage)}.{nameof(RealTimeService_DeviceUpdated)}, ex: {e.GetType()}, msg: {e.Message}");
                }

                if (!deviceSetting.Success)
                    _notifyPage.DisplayAlert("Error", "Could not change setting!");

                else if (_deviceViewModel.Discriminator == "Binary")
                {
                    _deviceViewModel.BinarySetting = deviceSetting.BinarySetting;
                    StatusLabel.Text = _deviceViewModel.Status;
                    await PutNewModel(_deviceViewModel);
                }
                else if (_deviceViewModel.Discriminator == "Continous")
                {
                    _deviceViewModel.ContinousSetting = deviceSetting.ContinousSetting;
                    StatusLabel.Text = _deviceViewModel.Status;
                    await PutNewModel(_deviceViewModel);
                }
                else
                {
                    Debug.WriteLine(
                        $"{nameof(DevicePage)}.{nameof(RealTimeService_DeviceUpdated)}, unknown view model {nameof(_deviceViewModel.Discriminator)} - {_deviceViewModel.Discriminator}");
                }
            }
        }

        private async Task PutNewModel(DeviceViewModel deviceViewModel)
        {
            var updateModel = new UpdateDeviceViewModel
            {
                Discriminator = deviceViewModel.Discriminator,
                BinarySetting = deviceViewModel.BinarySetting,
                ContinousSetting = deviceViewModel.ContinousSetting,
                DeviceId = deviceViewModel.DeviceId,
                Description = deviceViewModel.Description,
                GroupId = deviceViewModel.GroupId,
                Name = deviceViewModel.Name
            };

            if (!await _deviceRepository.PutDeviceAsync(updateModel))
            {
                Debug.WriteLine($"{nameof(DevicePage)}.{nameof(PutNewModel)}, could not put new model");
            }
            else
            {
                Debug.WriteLine($"{nameof(DevicePage)}.{nameof(PutNewModel)}, model updated");
            }
        }

        private async void ChangeSettingButton_OnClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null)
            {
                Debug.WriteLine($"{nameof(DevicePage)}.{nameof(ChangeSettingButton_OnClicked)}, sender is not {typeof(Button)}, found instead {sender?.GetType()}");
                return;
            }

            var newViewModel = new DeviceViewModel
            {
                DeviceType = _deviceViewModel.DeviceType,
                BinarySetting = _deviceViewModel.BinarySetting,
                ContinousSetting = _deviceViewModel.ContinousSetting,
                Discriminator = _deviceViewModel.Discriminator,
                Description = _deviceViewModel.Description,
                DeviceId = _deviceViewModel.DeviceId,
                GroupId = _deviceViewModel.GroupId,
                History = _deviceViewModel.History,
                Name = _deviceViewModel.Name
            };

            if (_deviceViewModel.Discriminator == "Binary")
            {
                button.IsEnabled = false;
                newViewModel.BinarySetting = !newViewModel.BinarySetting;

                var sent = await _realTimeService.UpdateDeviceAsync(new DeviceSetting(newViewModel));

                Debug.WriteLine($"{nameof(DevicePage)}.{nameof(ChangeSettingButton_OnClicked)}, sent request to change setting: {sent}");
            }
            else if (_deviceViewModel.Discriminator == "Continous")
            {
                button.IsEnabled = false;

                var entryText = ContinousSettingEntry.Text;

                double newValue;
                if (!double.TryParse(entryText, out newValue))
                {
                    _notifyPage.DisplayAlert("Error", "New value has unknown format!");
                    Debug.WriteLine($"{nameof(DevicePage)}.{nameof(ChangeSettingButton_OnClicked)}, new value has unknown format: {ContinousSettingEntry.Text}");
                }
                else
                {
                    newViewModel.ContinousSetting = newValue;
                    var sent = await _realTimeService.UpdateDeviceAsync(new DeviceSetting(newViewModel));
                    Debug.WriteLine($"{nameof(DevicePage)}.{nameof(ChangeSettingButton_OnClicked)}, sent request to change setting: {sent}");
                }
            }
            else
            {
                Debug.WriteLine($"{nameof(DevicePage)}.{nameof(ChangeSettingButton_OnClicked)}, unknown view model {nameof(_deviceViewModel.Discriminator)} - {_deviceViewModel.Discriminator}");
            }
        }
    }
}
