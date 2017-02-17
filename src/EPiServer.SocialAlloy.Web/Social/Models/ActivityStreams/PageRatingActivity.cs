using EPiServer.SocialAlloy.Web.Social.Adapters;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Represents the activity of a user rating being submitted in the site.
    /// </summary>
    public class PageRatingActivity : CommunityActivity
    {
        /// <summary>
        /// The rating value that was submitted by a user rating a page.
        /// </summary>
        public int Value { get; set; }

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