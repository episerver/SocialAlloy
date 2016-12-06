using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The ISocialActivityAdapter interface represents a set of operations used for adapting and 
    /// interpreting describing various types of Social activities in the Episerver Social sample.
    /// </summary>
    public interface ISocialActivityAdapter
    {
        /// <summary>
        /// Adpats a Composite feed item with SocialActivity as payload to a SocialFeedViewModel
        /// </summary>
        /// <param name="composite">the composite feed item with SocialActivity as payload to be adapted</param>
        /// <returns></returns>
        SocialFeedViewModel Adapt(Composite<FeedItem, SocialActivity> composite);

        /// <summary>
        /// Interprets a SocialActivity
        /// </summary>
        /// <param name="activity">the SocialActivity to interpret</param>
        void Visit(SocialActivity activity);

        /// <summary>
        /// Interprets a SocialCommentActivity
        /// </summary>
        /// <param name="activity">the SocialCommentActivity to interpret</param>
        void Visit(SocialCommentActivity activity);

        /// <summary>
        /// Interprets a SocialRatingActivity
        /// </summary>
        /// <param name="activity">the SocialRatingActivity to interpret</param>
        void Visit(SocialRatingActivity activity);
    }
}