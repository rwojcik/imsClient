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
            DisplayAlert("Error", "Not yet implemented...");
        }
        public void MissingHandler()
        {
            DisplayAlert("WTF", "Missing handler!");
        }

        public void DisplayAlert(string title, string message, string cancel = "Dissmiss")
        {
            _page.DisplayAlert(title, message, cancel);
        }


    }
}
