using Newtonsoft.Json;

namespace HandyCrab.Common.Entitys
{
    public class Login
    {
        [JsonProperty("login")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}