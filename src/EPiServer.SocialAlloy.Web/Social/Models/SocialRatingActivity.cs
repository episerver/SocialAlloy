using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public class SocialRatingActivity : SocialActivity
    {
        /// <summary>
        /// The rating value that was submitted by a user rating a page.
        /// </summary>
        public int Value { get; set; }

        public override void Accept(ISocialActivityVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}