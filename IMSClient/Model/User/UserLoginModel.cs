namespace IMSClient.Model.User
{
    public class UserLoginModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool SavePassord { get; set; } = true;
    }
}
