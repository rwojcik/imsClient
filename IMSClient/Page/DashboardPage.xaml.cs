
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IMSClient.Extension;
using IMSClient.Extension.Impl;
using IMSClient.Repository;
using IMSPrototyper.ViewModels;
using Xamarin.Forms;

namespace IMSClient.Page
{
    public partial class DashboardPage : ContentPage
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly INotifyPage _notifyPage;
        private readonly ObservableCollection<GroupViewModel> _groups;

        public DashboardPage(IUserRepository userRepository = null, IGroupRepository groupRepository = null)
        {
            _groupRepository = groupRepository;
            BindingContext = this;
            _groups = new ObservableCollection<GroupViewModel>();

            _userRepository = userRepository ?? DependencyService.Get<IUserRepository>();
            _groupRepository = groupRepository ?? DependencyService.Get<IGroupRepository>();

            _notifyPage = new NotifyPage(this);
            InitializeComponent();

            GroupsListView.ItemsSource = _groups;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if(_userRepository.IsLogged())
                _notifyPage.DisplayAlert("Error", "You are not logged in!");

            if (await DownloadValues())
                ChangeToShowingResults();
            else
                ChangeToNoResults();
        }

        private async Task<bool> DownloadValues()
        {
            var groups = await _groupRepository.GetGroupsAsync();

            foreach (var groupViewModel in groups)
            {
                _groups.Add(groupViewModel);
            }

            return groups.Any();
        }

        private void ChangeToShowingResults()
        {
            try
            {
                StatusIndicator.IsVisible = false;
                StatusIndicator.IsRunning = false;
                StatusIndicatorContentView.IsVisible = false;
                StatusLabel.Text = "Choose group to interact with";
            }
            catch (Exception e)
            {
                Debug.WriteLine($"ex: {e.GetType()}, msg: {e.Message}");
            }
        }

        public void ChangeToNoResults()
        {
            try
            {
                StatusIndicator.IsVisible = false;
                StatusIndicator.IsRunning = false;
                StatusIndicatorContentView.IsVisible = false;
                StatusLabel.Text = "There are no groups available";
            }
            catch (Exception e)
            {
                Debug.WriteLine($"ex: {e.GetType()}, msg: {e.Message}");
            }
        }

        private void OnOpen(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            DisplayAlert("Item selected", $"{e.SelectedItem}", "OK");

            try
            {
                var listView = sender as ListView;
                if (listView != null) listView.SelectedItem = null;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}
