using Newtonsoft.Json;
using System;

namespace HandyCrab.Common.Entitys
{
    public class User
    {
        [JsonProperty("_id")]
        public Guid Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}