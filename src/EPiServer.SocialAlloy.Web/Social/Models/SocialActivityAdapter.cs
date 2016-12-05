using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using System;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public class SocialActivityAdapter : ISocialActivityVisitor
    {
        private SocialActivityFeedViewModel feedModel;

        public SocialActivityFeedViewModel Adapt(Composite<FeedItem, SocialActivity> composite)
        {
            // Adapt any composite.Data stuff and populate view model accordingly…
            feedModel = new SocialActivityFeedViewModel
            {
                ActivityDate = composite.Data.ActivityDate,
                Actor = composite.Data.Actor.Id,
                Target = composite.Data.Target.Id
            };

            composite.Extension.Accept(this);

            return this.feedModel;
        }

        public void Visit(SocialCommentActivity activity)
        {
            feedModel.ActivityMessage = String.Format("commented on the page: {0}", activity.Body);
        }

        public void Visit(SocialRatingActivity activity)
        {
            feedModel.ActivityMessage = String.Format("rated the page: {0}", activity.Value.ToString());
        }

        public void Visit(SocialActivity activity)
        {
            activity.Accept(this);
        }
    }
}