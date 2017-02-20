using EPiServer.SocialAlloy.Web.Social.Adapters;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Represents the activity of posting a comment in the Episerver Social sample.
    /// </summary>
    public class PageCommentActivity : CommunityActivity
    {
        /// <summary>
        /// The body of the comment that was posted on a page.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Accepts an ICommunityActivityAdapter instance capable of interpreting this activity instance.
        /// </summary>
        /// <param name="adapter">an instance of ICommunityActivityAdapter</param>
        public override void Accept(ICommunityActivityAdapter adapter)
        {
            adapter.Visit(this);
        }
    }
}