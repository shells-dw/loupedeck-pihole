namespace Loupedeck.PiholePlugin.Models
{
    using System;
    using Newtonsoft.Json;

    public class Summary
    {
        [JsonProperty("domains_being_blocked")]
        public String DomainsBeingBlocked { get; set; }

        [JsonProperty("dns_queries_today")]
        public String DnsQueriesToday { get; set; }

        [JsonProperty("ads_blocked_today")]
        public String AdsBlockedToday { get; set; }

        [JsonProperty("ads_percentage_today")]
        public String AdsPercentageToday { get; set; }

        [JsonProperty("unique_domains")]
        public String UniqueDomains { get; set; }

        [JsonProperty("queries_forwarded")]
        public String QueriesForwarded { get; set; }

        [JsonProperty("queries_cached")]
        public String QueriesCached { get; set; }

        [JsonProperty("clients_ever_seen")]
        public String ClientsEverSeen { get; set; }

        [JsonProperty("unique_clients")]
        public String UniqueClients { get; set; }

        [JsonProperty("dns_queries_all_types")]
        public String DnsQueriesAllTypes { get; set; }

        [JsonProperty("reply_NODATA")]
        public String ReplyNoData { get; set; }

        [JsonProperty("reply_NXDOMAIN")]
        public String ReplyNxDomain { get; set; }

        [JsonProperty("reply_CNAME")]
        public String ReplyCname { get; set; }

        [JsonProperty("reply_IP")]
        public String ReplyIp { get; set; }

        [JsonProperty("privacy_level")]
        public String PrivacyLevel { get; set; }

        [JsonProperty("status")]
        public String Status { get; set; }

        [JsonProperty("gravity_last_updated")]
        public GravityLastUpdated GravityLastUpdated { get; set; }


    }
    public class Relative
    {
        [JsonProperty("days")]
        public Int32 Days { get; set; }

        [JsonProperty("hours")]
        public Int32 Hours { get; set; }

        [JsonProperty("minutes")]
        public Int32 Minutes { get; set; }
    }

    public class GravityLastUpdated
    {
        [JsonProperty("file_exists")]
        public Boolean FileExists { get; set; }

        [JsonProperty("absolute")]
        public String Absolute { get; set; }

        [JsonProperty("relative")]
        public Relative Relative { get; set; }
    }
}
