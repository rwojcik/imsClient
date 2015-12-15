using System;

namespace IMSClient.ViewModels
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string UserName { get; set; }

        public DateTime Expires { get; set; }
    }
}
