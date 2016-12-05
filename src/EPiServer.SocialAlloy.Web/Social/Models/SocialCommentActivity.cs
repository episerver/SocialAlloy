using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public class SocialCommentActivity : SocialActivity
    {
        /// <summary>
        /// The body of the comment that was posted on a page.
        /// </summary>
        public string Body { get; set; }

        public override void Accept(ISocialActivityVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}