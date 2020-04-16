using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace HandyCrab.Common.Entitys
{
    public class Barrier : IReadOnlyBarrier
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("solution")]
        public Solution[] Solution { get; set; }

        [JsonProperty("upvotes")]
        public int Upvotes { get; set; }

        [JsonProperty("downvotes")]
        public int Downvotes { get; set; }

        [JsonProperty("vote")]
        public int Vote { get; set; }
    }
}
