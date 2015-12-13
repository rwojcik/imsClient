using System;

namespace IMSClient.Model.User
{
    public class UserLoginModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool SavePassord { get; set; } = false;

        public string TokenType { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpires { get; set; }
    }
}
