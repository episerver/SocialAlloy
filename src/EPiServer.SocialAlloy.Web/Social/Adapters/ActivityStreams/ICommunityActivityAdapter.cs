using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Models;

namespace EPiServer.SocialAlloy.Web.Social.Adapters
{
    /// <summary>
    /// The ICommunityActivityAdapter interface represents a set of operations used for adapting and 
    /// interpreting describing various types of community activities in the site.
    /// </summary>
    public interface ICommunityActivityAdapter
    {
        /// <summary>
        /// Adapts a Composite feed item with CommunityActivity as payload to a CommunityFeedItemViewModel
        /// </summary>
        /// <param name="composite">the composite feed item with CommunityActivity as payload to be adapted</param>
        /// <returns>the adapted CommunityFeedItemViewModel</returns>
        CommunityFeedItemViewModel Adapt(Composite<FeedItem, CommunityActivity> composite);

        /// <summary>
        /// Interprets a CommunityActivity
        /// </summary>
        /// <param name="activity">the CommunityActivity to interpret</param>
        void Visit(CommunityActivity activity);

        /// <summary>
        /// Interprets a PageCommentActivity
        /// </summary>
        /// <param name="activity">the PageCommentActivity to interpret</param>
        void Visit(PageCommentActivity activity);

        /// <summary>
        /// Interprets a PageRatingActivity
        /// </summary>
        /// <param name="activity">the PageRatingActivity to interpret</param>
        void Visit(PageRatingActivity activity);
    }
}