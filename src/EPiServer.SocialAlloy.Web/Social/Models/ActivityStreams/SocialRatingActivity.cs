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
        /// Accepts an ISocialActivityVisitor instance capable of interpreting this activity instance.
        /// </summary>
        /// <param name="visitor">an instance of ISocialActivityVisitor</param>
        public override void Accept(ISocialActivityAdapter visitor)
        {
            visitor.Visit(this);
        }
    }
}