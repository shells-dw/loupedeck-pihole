namespace Loupedeck.PiholePlugin.Models
{
    using System;

    public class Summary
    {
        public String domains_being_blocked { get; set; }

        public String dns_queries_today { get; set; }

        public String ads_blocked_today { get; set; }

        public String ads_percentage_today { get; set; }

        public String unique_domains { get; set; }

        public String queries_forwarded { get; set; }

        public String queries_cached { get; set; }

        public String clients_ever_seen { get; set; }

        public String unique_clients { get; set; }

        public String dns_queries_all_types { get; set; }

        public String reply_nodata { get; set; }

        public String reply_nxdomain { get; set; }

        public String reply_cname { get; set; }

        public String reply_ip { get; set; }

        public String privacy_level { get; set; }

        public String status { get; set; }
  
        public GravityLastUpdated gravity_last_updated { get; set; }


    }
    public class Relative
    {
        public Int32 days { get; set; }

        public Int32 hours { get; set; }

        public Int32 minutes { get; set; }
    }

    public class GravityLastUpdated
    {
        public Boolean file_exists { get; set; }

        public String absolute { get; set; }

        public Relative relative { get; set; }
    }
}
