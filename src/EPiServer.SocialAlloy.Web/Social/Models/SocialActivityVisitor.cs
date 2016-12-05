using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public class SocialActivityVisitor : ISocialActivityVisitor
    {
        private string activityDescription;
        public string ActivityDescription {
            get
            {
                return activityDescription;
            }
        }

        public void Visit(SocialCommentActivity activity)
        {
            activityDescription = String.Format("commented on the page: {0}", activity.Body);
        }

        public void Visit(SocialRatingActivity activity)
        {
            activityDescription = String.Format("rated the page: {0}", activity.Value.ToString());
        }
    }
}