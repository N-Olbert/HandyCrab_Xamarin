﻿using Newtonsoft.Json;

namespace HandyCrab.Common.Entitys
{
    public class SolutionBase
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("upvotes")]
        public int Upvotes { get; set; }

        [JsonProperty("downvotes")]
        public int Downvotes { get; set; }

        [JsonProperty("vote")]
        public Vote Vote { get; set; }
    }
}
