using System.Collections.Generic;
using Newtonsoft.Json;

namespace HandyCrab.Common.Entitys
{
    public class BarrierBase
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

        [JsonProperty("picturePath")]
        public string Picture { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("solutions")]
        public List<Solution> Solutions { get; set; }

        [JsonProperty("upvotes")]
        public int Upvotes { get; set; }

        [JsonProperty("downvotes")]
        public int Downvotes { get; set; }

        [JsonProperty("vote")]
        public Vote Vote { get; set; }
    }
}