using EPiServer.SocialAlloy.Web.Social.Adapters;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Represents the activity of a user rating bring submitted for an item in the Episerver Social sample.
    /// </summary>
    public class SocialRatingActivity : SocialActivity
    {
        /// <summary>
        /// The rating value that was submitted by a user rating a page.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Accepts an ISocialActivityAdapter instance capable of interpreting this activity instance.
        /// </summary>
        /// <param name="adapter">an instance of ISocialActivityAdapter</param>
        public override void Accept(ISocialActivityAdapter adapter)
        {
            adapter.Visit(this);
        }
    }
}