using Newtonsoft.Json;

namespace HandyCrab.Common.Entitys
{
    public class UserWithPassword : User
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("createToken")]
        public bool CreateToken { get; set; } = true;
    }
}