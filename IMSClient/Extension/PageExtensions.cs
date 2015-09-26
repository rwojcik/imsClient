namespace IMSClient.Extension
{
    public static class PageExtensions
    {
        public static void NotYetImplemented(this Xamarin.Forms.Page page)
        {
            page.DisplayAlert("Error", "Not yet implemented...", "Dismiss");
        }
        public static void MissingHandler(this Xamarin.Forms.Page page)
        {
            page.DisplayAlert("WTF", "Missing handler!", "Dismiss");
        }

    }
}
