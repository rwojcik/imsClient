using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMSClient.Extension;
using IMSClient.Extension.Impl;
using IMSClient.Repository;
using IMSClient.ViewModels;
using Xamarin.Forms;

namespace IMSClient.Page
{
    public delegate void DeviceChoose(object sender, DeviceChooseEventArgs e);

    public class DeviceChooseEventArgs :EventArgs
    {
        public DeviceViewModel DeviceViewModel { get; }

        public DeviceChooseEventArgs(DeviceViewModel device)
        {
            DeviceViewModel = device;
        }
    }

    public partial class GroupPage : ContentPage
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly INotifyPage _notifyPage;
        private readonly ObservableCollection<DeviceViewModel> _devices;
        private readonly GroupViewModel _groupViewModel;
        public event DeviceChoose DeviceChoose;

        public GroupPage(GroupViewModel groupViewModel = null, IUserRepository userRepository = null, IGroupRepository groupRepository = null, IDeviceRepository deviceRepository = null)
        {
            _groupViewModel = groupViewModel;
            BindingContext = this;
            _devices = new ObservableCollection<DeviceViewModel>();

            _userRepository = userRepository ?? DependencyService.Get<IUserRepository>();
            _groupRepository = groupRepository ?? DependencyService.Get<IGroupRepository>();
            _deviceRepository = deviceRepository ?? DependencyService.Get<IDeviceRepository>();

            _notifyPage = new NotifyPage(this);

            InitializeComponent();

            DevicesListView.ItemsSource = _devices;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Debug.WriteLine($"GroupPage-OnAppearing, group id: {_groupViewModel?.GroupId}");

            if (!_userRepository.IsLogged())
                _notifyPage.DisplayAlert("Error", "You are not logged in!");

            if (_groupViewModel?.GroupId == 0)
                _notifyPage.DisplayAlert("Error", "Group identifier is not specified!");

            if (await DownloadValues())
                ChangeToShowingResults();
            else
                ChangeToNoResults();
        }

        private async Task<bool> DownloadValues()
        {
            Debug.WriteLine($"GroupPage-DownloadValues, before download");

            _devices.Clear();

            try
            {
                var devices = await _deviceRepository.GetDevicesAsync(_groupViewModel?.GroupId ?? 0);

                Debug.WriteLine($"GroupPage-DownloadValues, downloaded: {devices.Count} device(s)");

                foreach (var deviceViewModel in devices)
                {
                    _devices.Add(deviceViewModel);
                }
                
                return devices.Any();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"GroupPage-DownloadValues, ex: {e.GetType()}, msg: {e.Message}");
                return false;
            }
        }

        private void ChangeToShowingResults()
        {
            try
            {
                StatusIndicator.IsVisible = false;
                StatusIndicator.IsRunning = false;
                StatusIndicatorContentView.IsVisible = false;
                StatusLabel.Text = $"Devices in {_groupViewModel.Name}";
            }
            catch (Exception e)
            {
                Debug.WriteLine($"ex: {e.GetType()}, msg: {e.Message}");
            }
        }

        private void ChangeToNoResults()
        {
            try
            {
                StatusIndicator.IsVisible = false;
                StatusIndicator.IsRunning = false;
                StatusIndicatorContentView.IsVisible = false;
                StatusLabel.Text = "There are no devices available";
            }
            catch (Exception e)
            {
                Debug.WriteLine($"ex: {e.GetType()}, msg: {e.Message}");
            }
        }

        private async void OnOpen(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            //DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");

            var device = mi?.CommandParameter as DeviceViewModel;

            if (device != null)
            {
                DeviceChoose?.Invoke(this, new DeviceChooseEventArgs(device));
            }
        }

        private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            //DisplayAlert("Item selected", $"{e.SelectedItem}", "OK");

            try
            {
                var listView = sender as ListView;
                if (listView != null) listView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.GetType()}, msg: {ex.Message}");
            }

            var device = e.SelectedItem as DeviceViewModel;

            if (device != null)
            {
                DeviceChoose?.Invoke(this, new DeviceChooseEventArgs(device));
            }
        }
    }
}
