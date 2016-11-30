using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public class SocialRatingFilter
    {
        public string Target { get; set; }

        public string Rater { get; set; }
    }
}