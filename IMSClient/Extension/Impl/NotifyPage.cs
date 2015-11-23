using IMSClient.Extension.Impl;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotifyPage))]
namespace IMSClient.Extension.Impl
{
    public class NotifyPage : INotifyPage
    {
        private readonly Xamarin.Forms.Page _page;

        public NotifyPage(Xamarin.Forms.Page page)
        {
            _page = page;
        }

        public void NotYetImplemented()
        {
            _page.DisplayAlert("Error", "Not yet implemented...", "Dismiss");
        }
        public void MissingHandler()
        {
            _page.DisplayAlert("WTF", "Missing handler!", "Dismiss");
        }

        public void DisplayAlert(string title, string message, string cancel)
        {
            _page.DisplayAlert(title, message, cancel);
        }
    }
}
