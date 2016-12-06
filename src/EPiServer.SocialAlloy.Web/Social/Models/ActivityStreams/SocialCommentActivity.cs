namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Represents the activity of posting a comment in the Episerver Social sample.
    /// </summary>
    public class SocialCommentActivity : SocialActivity
    {
        /// <summary>
        /// The body of the comment that was posted on a page.
        /// </summary>
        public string Body { get; set; }

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