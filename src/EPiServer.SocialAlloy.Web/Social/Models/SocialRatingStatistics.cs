using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public class SocialRatingStatistics
    {
        public decimal Average { get; set; }

        public long TotalCount { get; set; }
    }
}