
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IMSClient.Respository;
using IMSPrototyper.ViewModels;
using Xamarin.Forms;

namespace IMSClient.Page
{
    public partial class DashboardPage : ContentPage
    {
        private IUserRepository _userRepository;
        private IValuesRepository _valuesRepository;
        private IList<GroupViewModel> _groups;


        public DashboardPage(IUserRepository _userRepository = null, IValuesRepository _valuesRepository = null)
        {
            _groups = new ObservableCollection<GroupViewModel>();

            this._userRepository = _userRepository ?? DependencyService.Get<IUserRepository>();
            this._valuesRepository = _valuesRepository ?? DependencyService.Get<IValuesRepository>();
            InitializeComponent();

            try
            {
                GroupsListView.ItemsSource = _groups;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"ex: {e.GetType()}, msg: {e.Message}");
            }

            Task.Factory.StartNew(AddDebugValues);
        }

        private void AddDebugValues()
        {
            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                try
                {
                    _groups.Add(new GroupViewModel
                    {
                        Name = "Test1",
                        DevicesIds = new long[] { 3, 4, 6, 8, 45, 84, }
                    });
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"ex: {e.GetType()}, msg: {e.Message}");
                }
                return false;
            });

            Device.StartTimer(TimeSpan.FromSeconds(4), () =>
            {
                try
                {
                    _groups.Add(new GroupViewModel
                    {
                        Name = "Test2",
                        DevicesIds = new long[] { 3, 4, 6, 84, }
                    });

                    _groups.Add(new GroupViewModel
                    {
                        Name = "Test3",
                        DevicesIds = new long[] { 3, 4, 45, 84, }
                    });

                    _groups.Add(new GroupViewModel
                    {
                        Name = "Test4",
                        DevicesIds = new long[] { 3, 4, 6, 8 }
                    });
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"ex: {e.GetType()}, msg: {e.Message}");
                }
                return false;
            });

            Device.StartTimer(TimeSpan.FromSeconds(6), () =>
            {
                try
                {
                    var last = _groups.LastOrDefault();

                    if (last != null)
                        _groups.Remove(last);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"ex: {e.GetType()}, msg: {e.Message}");
                }

                return false;
            });


        }

        public void OnMore(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DisplayAlert("Item selected", $"{e.SelectedItem.ToString()}", "OK");
        }
    }
}
