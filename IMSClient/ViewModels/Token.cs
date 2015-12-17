using System;
using Newtonsoft.Json;

namespace IMSClient.ViewModels
{
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        
        public string UserName { get; set; }

        [JsonProperty(".expires")]
        public DateTime Expires { get; set; }
    }
}
