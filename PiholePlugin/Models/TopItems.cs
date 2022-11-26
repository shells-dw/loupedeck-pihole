namespace PiHoleApiClient.Models
{
    using System;
    using Newtonsoft.Json;

    using System.Collections.Generic;

    public class TopItems
    {
        [JsonProperty("top_queries")]
        public Dictionary<String, Int64> TopQueries { get; set; }

        [JsonProperty("top_ads")]
        public Dictionary<String, Int64> TopAds { get; set; }
    }
}