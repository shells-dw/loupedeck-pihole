namespace Loupedeck.PiholePlugin.Models
{
    using System;
    using Newtonsoft.Json;

    public class PiStatus
    {
        [JsonProperty("status")]
        public String Status { get; set; }
    }
}
